using Memory;
using System;
using System.Threading;
using System.Numerics;
using System.Linq;

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
        public float endPointDistance = 100;

        private DateTime startTime;

        public TimerManager()
        {
            Thread timerLoopThread = new(TimerLoop);
            timerLoopThread.IsBackground = true;
            timerLoopThread.Start();
        }

        private void TimerLoop()
        {
            while (true)
            {
                if (mem.OpenProcess("meridian"))
                {
                    if (TimerShouldReset())
                    {
                        timerState = TIMER_STATE.READY;
                    }

                    if (timerState == TIMER_STATE.READY && TimerShouldStart())
                    {
                        startTime = DateTime.Now;
                        timerState = TIMER_STATE.STARTED;
                    }

                    if (timerState == TIMER_STATE.STARTED)
                    {
                        onTimerTick?.Invoke(DateTime.Now - startTime);
                    }

                    if (TimerShouldStop())
                    {
                        timerState = TIMER_STATE.STOPPED;
                        onTimerEnd?.Invoke(DateTime.Now - startTime);
                    }
                }

                Thread.Sleep(1000 / 30);
            }
        }

        private bool TimerShouldReset()
        {
            return mem.ReadInt(MemoryAddresses.loading) == 1
                || StateLists.deathStates.Contains(mem.ReadInt(MemoryAddresses.bilboState));
        }

        private bool TimerShouldStart()
        {
            switch (startCondition)
            {
                case START_CONDITION.NONE:
                    break;

                case START_CONDITION.LEVEL_START:
                    return mem.ReadInt(MemoryAddresses.outOfLevelState) == 10;

                case START_CONDITION.MOVEMENT:
                    return StateLists.movementStates.Contains(mem.ReadInt(MemoryAddresses.bilboState));
            }

            return false;
        }

        private bool TimerShouldStop()
        {
            switch (endCondition)
            {
                case END_CONDITION.NONE:
                    break;

                case END_CONDITION.NEXT_LEVEL:
                    return mem.ReadInt(MemoryAddresses.outOfLevelState) != 10;

                case END_CONDITION.POINT_REACHED:
                    float x = mem.ReadFloat(MemoryAddresses.bilboCoordsX);
                    float y = mem.ReadFloat(MemoryAddresses.bilboCoordsY);
                    float z = mem.ReadFloat(MemoryAddresses.bilboCoordsZ);

                    Vector3 bilboPosition = new(x, y, z);

                    return Vector3.Distance(bilboPosition, endPointPosition) < endPointDistance;
            }

            return false;
        }

        public void SetEndPointPosition()
        {
            float x = mem.ReadFloat(MemoryAddresses.bilboCoordsX);
            float y = mem.ReadFloat(MemoryAddresses.bilboCoordsY);
            float z = mem.ReadFloat(MemoryAddresses.bilboCoordsZ);

            endPointPosition = new(x, y, z);
        }
    }
}
