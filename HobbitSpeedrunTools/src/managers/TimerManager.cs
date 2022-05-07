using Memory;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Numerics;

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

        public readonly Mem mem = new();

        public Action<TimeSpan>? onTimerTick;
        public Action<TimeSpan>? onTimerEnd;

        public START_CONDITION startCondition = START_CONDITION.MOVEMENT;
        public END_CONDITION endCondition = END_CONDITION.POINT_REACHED;

        private DateTime startTime;
        private bool timer;
        private bool stopped;

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
                    if (stopped)
                    {
                        stopped = mem.ReadInt(MemoryAddresses.outOfLevelState) == 10;
                    }
                    else
                    {
                        if (TimerShouldStop())
                        {
                            timer = false;
                            stopped = true;
                            onTimerEnd?.Invoke(DateTime.Now - startTime);
                            continue;
                        }

                        if (!timer && TimerShouldStart())
                        {
                            startTime = DateTime.Now;
                            timer = true;
                        }

                        if (timer)
                        {
                            onTimerTick?.Invoke(DateTime.Now - startTime);
                        }
                    }
        

                }

                Thread.Sleep(1000 / 30);
            }
        }

        private bool TimerShouldStart()
        {
            switch (startCondition)
            {
                case START_CONDITION.NONE:
                    break;

                case START_CONDITION.LEVEL_START:
                    int outOfLevelState = mem.ReadInt(MemoryAddresses.outOfLevelState);
                    return outOfLevelState == 10;

                case START_CONDITION.MOVEMENT:
                    int bilboState = mem.ReadInt(MemoryAddresses.bilboState);
                    return bilboState == 3 || bilboState == 5 || bilboState == 15;
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
                    int outOfLevelState = mem.ReadInt(MemoryAddresses.outOfLevelState);
                    return outOfLevelState != 10;

                case END_CONDITION.POINT_REACHED:
                    float x = mem.ReadFloat(MemoryAddresses.bilboCoordsX);
                    float y = mem.ReadFloat(MemoryAddresses.bilboCoordsY);
                    float z = mem.ReadFloat(MemoryAddresses.bilboCoordsZ);

                    Vector3 position = new Vector3(x, y, z);
                    Vector3 endPoint = new Vector3(-1746, -227, -1898);

                    return Vector3.Distance(position, endPoint) < 100;
            }

            return false;
        }
    }
}
