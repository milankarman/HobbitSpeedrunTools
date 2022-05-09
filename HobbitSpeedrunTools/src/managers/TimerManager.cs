using Memory;
using System;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace HobbitSpeedrunTools
{
    public class TimerManager
    {
        public enum TIMER_MODE
        {
            OFF,
            FULL_LEVEL,
            MOVE_TO_POINT,
        }

        public readonly Mem mem = new();

        public TIMER_MODE mode;

        public Action<TimeSpan>? onTimerTick;
        public Action<TimeSpan>? onNewBestTime;

        public Vector3? endPointPosition;
        public int endPointDistance = 150;

        public TimeSpan? bestTime;

        private DateTime startTime;

        private bool timerStarted = false;
        private bool timerBlocked = false;
        private bool readyToReset = false;
        private int startLevel = 0;

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
                    switch (mode)
                    {
                        case TIMER_MODE.OFF:
                            break;

                        case TIMER_MODE.FULL_LEVEL:
                            HandleLevelTimer();
                            break;

                        case TIMER_MODE.MOVE_TO_POINT:
                            HandlePointTimer();
                            break;
                    }

                }
            }
        }

        private void HandleLevelTimer()
        {
            if (timerBlocked)
            {
                if (mem.ReadInt(MemoryAddresses.loadFinished) == 1)
                {
                    readyToReset = true;
                }

                if (readyToReset && mem.ReadInt(MemoryAddresses.outOfLevelState) == 17)
                {
                    timerBlocked = false;
                    readyToReset = false;
                }
                else
                {
                    return;
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

                if (bestTime != null || DateTime.Now - startTime < bestTime)
                {
                    onNewBestTime?.Invoke(DateTime.Now - startTime);
                }
            }

            if (timerStarted)
            {
                onTimerTick?.Invoke(DateTime.Now - startTime);
            }
        }

        private void HandlePointTimer()
        {
            if (endPointPosition == null) return;

            if (timerBlocked)
            {
                if (StateLists.deathStates.Contains(mem.ReadInt(MemoryAddresses.bilboState)))
                {
                    timerBlocked = false;
                }
                else
                {
                    return;
                }
            }

            if (timerStarted == false && StateLists.movementStates.Contains(mem.ReadInt(MemoryAddresses.bilboState)))
            {
                startTime = DateTime.Now;
                timerStarted = true;
            }

            if (timerStarted)
            {
                float x = mem.ReadFloat(MemoryAddresses.bilboCoordsX);
                float y = mem.ReadFloat(MemoryAddresses.bilboCoordsY);
                float z = mem.ReadFloat(MemoryAddresses.bilboCoordsZ);

                Vector3 bilboPosition = new(x, y, z);

                if (Vector3.Distance(bilboPosition, (Vector3)endPointPosition) < endPointDistance)
                {
                    timerStarted = false;
                    timerBlocked = true;

                    if (bestTime != null || DateTime.Now - startTime < bestTime)
                    {
                        onNewBestTime?.Invoke(DateTime.Now - startTime);
                    }
                }
            }

            if (timerStarted)
            {
                onTimerTick?.Invoke(DateTime.Now - startTime);
            }
        }

        public void SetTimerMode(TIMER_MODE _mode)
        {
            mode = _mode;
            ResetTimerStates();
        }

        public void ResetTimerStates()
        {
            timerStarted = false;
            timerBlocked = false;
            readyToReset = false;
            startLevel = 0;
            bestTime = null;
        }

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
