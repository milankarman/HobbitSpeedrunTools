using Memory;
using System;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace HobbitSpeedrunTools
{
    public class TimerManager
    {
        public enum START_CONDITION
        {
            NONE,
            LEVEL_START,
            MOVEMENT,
        }

        public enum END_CONDITION
        {
            NONE,
            NEXT_LEVEL,
            POINT_REACHED,
        }

        public enum TIMER_STATE
        {
            READY,
            STARTED,
            STOPPED,
        }

        public readonly Mem mem = new();
        public bool stopped = true;

        public Action<TimeSpan>? onTimerTick;
        public Action<TimeSpan>? onTimerEnd;

        public TIMER_STATE timerState;
        public START_CONDITION startCondition;
        public END_CONDITION endCondition;

        public Vector3 endPointPosition = new(0, 0, 0);
        public int endPointDistance = 150;

        private DateTime startTime;

        public TimerManager()
        {
            Thread timerLoopThread = new(TimerLoop);
            timerLoopThread.IsBackground = true;
            timerLoopThread.Start();
        }

        private void TimerLoop()
        {
            bool timerStarted = false;
            bool timerBlocked = false;
            bool readyToUnblock = false;
            int startLevel = 0;

            while (true)
            {
                if (mem.OpenProcess("meridian"))
                {
                    if (timerBlocked)
                    {
                        if (mem.ReadInt(MemoryAddresses.loadFinished) == 1)
                        {
                            readyToUnblock = true;
                        }

                        if (readyToUnblock && mem.ReadInt(MemoryAddresses.outOfLevelState) == 17)
                        {
                            timerBlocked = false;
                            readyToUnblock = false;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (timerStarted == false && mem.ReadInt(MemoryAddresses.loadFinished) == 1)
                    {
                        startTime = DateTime.Now;
                        startLevel = mem.ReadInt(MemoryAddresses.currentLevelID);
                        timerStarted = true;
                    }

                    if (timerStarted && mem.ReadInt(MemoryAddresses.currentLevelID) != startLevel)
                    {
                        timerStarted = false;
                        timerBlocked = true;
                        onTimerEnd?.Invoke(DateTime.Now - startTime);
                    }

                    if (timerStarted)
                    {
                        onTimerTick?.Invoke(DateTime.Now - startTime);
                    }
                }
            }
        }


        //private bool TimerShouldReset()
        //{
        //    //return StateLists.deathStates.Contains(mem.ReadInt(MemoryAddresses.bilboState))
        //    //    || startLevel != mem.ReadInt(MemoryAddresses.currentLevelID);
        //}

        //private bool TimerShouldStart()
        //{
        //    switch (startCondition)
        //    {
        //        case START_CONDITION.NONE:
        //            break;

        //        case START_CONDITION.LEVEL_START:
        //            return mem.ReadInt(MemoryAddresses.loadFinished) == 1;

        //        case START_CONDITION.MOVEMENT:
        //            return StateLists.movementStates.Contains(mem.ReadInt(MemoryAddresses.bilboState));
        //    }

        //    return false;
        //}

        //private bool TimerShouldStop()
        //{
        //    switch (endCondition)
        //    {
        //        case END_CONDITION.NONE:
        //            break;

        //        case END_CONDITION.NEXT_LEVEL:
        //            int newLevel = mem.ReadInt(MemoryAddresses.currentLevelID);
        //            if (startLevel != newLevel)
        //            {
        //                newLevel = startLevel;
        //                return true;
        //            }
        //            return false;

        //        case END_CONDITION.POINT_REACHED:
        //            float x = mem.ReadFloat(MemoryAddresses.bilboCoordsX);
        //            float y = mem.ReadFloat(MemoryAddresses.bilboCoordsY);
        //            float z = mem.ReadFloat(MemoryAddresses.bilboCoordsZ);

        //            Vector3 bilboPosition = new(x, y, z);

        //            return Vector3.Distance(bilboPosition, endPointPosition) < endPointDistance;
        //    }

        //    return false;
        //}

        public void SetEndPointPosition()
        {
            if (mem.OpenProcess("meridian"))
            {
                float x = mem.ReadFloat(MemoryAddresses.bilboCoordsX);
                float y = mem.ReadFloat(MemoryAddresses.bilboCoordsY);
                float z = mem.ReadFloat(MemoryAddresses.bilboCoordsZ);

                endPointPosition = new(x, y, z);
            }
        }
    }
}
