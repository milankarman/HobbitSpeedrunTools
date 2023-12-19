using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace HobbitSpeedrunTools
{
    public class StatusManager
    {
        private CheatManager cheatManager;
        private SaveManager saveManager;
        private TimerManager timerManager;

        private TimeSpan timerTime;
        private TimeSpan timerBestTime;

        public StatusManager(CheatManager _cheatManager, SaveManager _saveManager, TimerManager _timerManager)
        {
            cheatManager = _cheatManager;
            saveManager = _saveManager;
            timerManager = _timerManager;

            timerManager.onTimerTick += (time) => timerTime = time;
            timerManager.onNewBestTime += (time) => timerBestTime = time;
        }

        public string GetStatusText()
        {
            string status = $"HST {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}";

            List<string> cheats = cheatManager.GetToggleCheatList();

            if (cheats.Count > 0) status += "\nCheats: ";

            for (int i = 0; i < cheats.Count; i++)
            {
                status += cheats[i];

                if (i < cheats.Count - 1)
                {
                    status += " ";
                }
            }

            if (saveManager.SaveCollectionIndex > 0)
            {
                status += $"\nSave: {saveManager.SaveCollectionIndex}-{saveManager.SaveIndex + 1}";
            }

            if (timerManager.mode != TimerManager.TIMER_MODE.OFF)
            {
                status += $"\nT:{timerTime:mm\\:ss\\.ff} B:{timerBestTime:mm\\:ss\\.ff}";
            }

            status = status.PadRight(65, ' ')[..65];

            return status;
        }
    }
}
