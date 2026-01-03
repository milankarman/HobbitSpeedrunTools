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

        private readonly int timerUpdateRate = 1000 / 120;

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
            // Conditions for timer start
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
                int currentLevelID = mem.ReadInt(MemoryAddresses.currentLevelID);
                int OoLState = mem.ReadInt(MemoryAddresses.outOfLevelState);
                int onCutscene = mem.ReadInt(MemoryAddresses.onCutscene);
                int cutsceneID = mem.ReadInt(MemoryAddresses.cutsceneID);
                int load = mem.ReadInt(MemoryAddresses.load);

                // Reset conditions, shoutouts to Shocky.
                if (currentLevelID == selectedLevel)
                {
                    // Reset condition for dream world.
                    if (selectedLevel == 0 && OoLState == 20 && currentTime.TotalSeconds >= 0.05d) timerStarted = false;

                    // Reset condition for AUP.
                    if (selectedLevel == 1 && OoLState == 17) timerStarted = false;

                    // Reset condition for all other level segments.
                    if (OoLState == 12) timerStarted = false;
                }

                // Catch all reset condition for quick reload
                if (load == 1) timerStarted = false;

                if (currentLevelID > 10 && onCutscene == 1 && cutsceneID == 0x3853B400
                    || OoLState == 19 && currentLevelID > selectedLevel)
                {
                    timerStarted = false;

                    previousTimes.Add(currentTime);

                    onUpdateAverageTime?.Invoke(GetAverageTime());
                    onTimerTick?.Invoke(currentTime);

                    if (bestTime == null || currentTime < bestTime)
                    {
                        bestTime = currentTime;
                        onNewBestTime?.Invoke(currentTime);
                    }
                }

                if (currentLevelID == selectedLevel)
                {
                    // If for some reason we load a save that's past the levels or before the start level in the segment or IL, we reset.
                    if (mem.ReadInt(MemoryAddresses.currentLevelID) > selectedLevel + 1
                        || mem.ReadInt(MemoryAddresses.currentLevelID) < selectedLevel) timerStarted = false;
                }
            }

            if (timerStarted)
            {
                onTimerTick?.Invoke(currentTime);
            }
        }

        private void HandlePointTimer()
        {
            if (endPointPosition == null) return;

            int bilboState = mem.ReadInt(MemoryAddresses.bilboState);
            int loadFinished = mem.ReadInt(MemoryAddresses.loadFinished);

            if (timerBlocked)
            {
                if (StateLists.deathStates.Contains(bilboState) || loadFinished == 1)   
                {
                    timerBlocked = false;
                }
                else
                {
                    return;
                }
            }

            if (timerStarted == false && StateLists.movementStates.Contains(bilboState))
            {
                startTime = DateTime.Now;
                timerStarted = true;
                return;
            }

            TimeSpan currentTime = DateTime.Now - startTime;

            if (timerStarted)
            {
                if (StateLists.deathStates.Contains(bilboState) || loadFinished == 1)
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
