using Memory;
using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Collections.Generic;

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
        public Action<TimeSpan>? onUpdateAverageTime;

        public int selectedLevel = 0;

        public Vector3? endPointPosition;
        public int endPointDistance = 150;

        public TimeSpan? bestTime;

        private readonly int timerUpdateRate = 1000 / 60;

        private DateTime startTime;
        private readonly List<TimeSpan> previousTimes = [];
        private bool timerStarted = false;
        private bool timerBlocked = false;

        public TimerManager()
        {
            Thread timerLoopThread = new(TimerLoop)
            {
                IsBackground = true
            };

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

                Thread.Sleep(timerUpdateRate);
            }
        }

        private void HandleLevelTimer()
        {
            if (timerStarted == false
                && mem.ReadInt(MemoryAddresses.currentLevelID) == selectedLevel
                && mem.ReadInt(MemoryAddresses.loadFinished) == 1
                && StateLists.levelLoadedOutOfLevelStates.Contains(mem.ReadInt(MemoryAddresses.outOfLevelState)))
            {
                startTime = DateTime.Now;
                timerStarted = true;
                return;
            }

            TimeSpan currentTime = DateTime.Now - startTime;

            if (timerStarted)
            {
                if (mem.ReadInt(MemoryAddresses.currentLevelID) > 10
                    && mem.ReadInt(MemoryAddresses.onCutscene) == 1
                    && mem.ReadInt(MemoryAddresses.cutsceneID) == 0x3853B400
                    || mem.ReadInt(MemoryAddresses.outOfLevelState) == 19
                    && mem.ReadInt(MemoryAddresses.currentLevelID) > selectedLevel)
                {
                    timerStarted = false;
                    timerBlocked = true;

                    previousTimes.Add(currentTime);

                    onUpdateAverageTime?.Invoke(GetAverageTime());
                    onTimerTick?.Invoke(currentTime);

                    if (bestTime == null || currentTime < bestTime)
                    {
                        bestTime = currentTime;
                        onNewBestTime?.Invoke(currentTime);
                    }
                }

                // Reset conditions, shoutouts to Shocky.
                if (mem.ReadInt(MemoryAddresses.currentLevelID) == selectedLevel)
                {
                    // Reset condition for dream world.
                    if (selectedLevel == 0 && mem.ReadInt(MemoryAddresses.outOfLevelState) == 20 && currentTime.TotalSeconds >= 0.05d) timerStarted = false;

                    // Reset condition for AUP.
                    if (selectedLevel == 1 && mem.ReadInt(MemoryAddresses.outOfLevelState) == 17) timerStarted = false;

                    // Reset condition for all other level segments.
                    if (mem.ReadInt(MemoryAddresses.outOfLevelState) == 12) timerStarted = false; 
                }

                // If for some reason we load a save thats passed the levels or before the start level in the segment or IL, we reset.
                if (mem.ReadInt(MemoryAddresses.currentLevelID) > selectedLevel + 1
                    || mem.ReadInt(MemoryAddresses.currentLevelID) < selectedLevel) timerStarted = false;
            }

            if (timerStarted)
            {
                onTimerTick?.Invoke(currentTime);
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
                return;
            }

            TimeSpan currentTime = DateTime.Now - startTime;

            if (timerStarted)
            {
                if (StateLists.deathStates.Contains(mem.ReadInt(MemoryAddresses.bilboState)))
                {
                    timerStarted = false;
                    return;
                }

                float x = mem.ReadFloat(MemoryAddresses.bilboCoordsX);
                float y = mem.ReadFloat(MemoryAddresses.bilboCoordsY);
                float z = mem.ReadFloat(MemoryAddresses.bilboCoordsZ);

                Vector3 bilboPosition = new(x, y, z);

                if (Vector3.Distance(bilboPosition, (Vector3)endPointPosition) < endPointDistance)
                {
                    timerStarted = false;
                    timerBlocked = true;

                    previousTimes.Add(currentTime);

                    onUpdateAverageTime?.Invoke(GetAverageTime());
                    onTimerTick?.Invoke(currentTime);

                    if (bestTime == null || currentTime < bestTime)
                    {
                        bestTime = DateTime.Now - startTime;
                        onNewBestTime?.Invoke(currentTime);
                    }
                }
            }

            if (timerStarted)
            {
                onTimerTick?.Invoke(currentTime);
            }
        }

        private TimeSpan GetAverageTime()
        {
            TimeSpan total = new();

            foreach (TimeSpan time in previousTimes)
            {
                total += time;
            }

            return total / previousTimes.Count;
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
            bestTime = null;

            onTimerTick?.Invoke(new TimeSpan());
            onNewBestTime?.Invoke(new TimeSpan());
            ResetAverageTime();
        }

        public void ResetAverageTime()
        {
            previousTimes.Clear();
            onUpdateAverageTime?.Invoke(new TimeSpan());
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

            ResetTimerStates();
            timerBlocked = true;
        }
    }
}
