using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace HobbitSpeedrunTools
{
    public class StatusManager
    {
        public CheatManager cheatManager;
        public SaveManager saveManager;
        public TimerManager timerManager;

        public TimeSpan timerTime;

        public StatusManager(CheatManager _cheatManager, SaveManager _saveManager, TimerManager _timerManager)
        {
            cheatManager = _cheatManager;
            saveManager = _saveManager;
            timerManager = _timerManager;

            timerManager.onTimerTick += (time) => timerTime = time;
        }

        public string GetStatusText()
        {
            string status = $"HST {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion}";

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
                status += $"\n{timerTime:mm\\:ss\\.ff}";
            }

            status = status.PadRight(65, ' ')[..65];

            return status;
        }
    }
}
