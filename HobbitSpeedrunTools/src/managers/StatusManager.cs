using System;
using System.Collections.Generic;

namespace HobbitSpeedrunTools
{
    public static class StatusManager
    {
        public static string GetStatusText()
        {
            string status = $"HobbitSpeedrunTools {About.version}";

            List<string> cheats = new List<string>();

            if (MemoryManager.devMode == Enums.CheatStatus.IS_ENABLED) cheats.Add("D");
            if (MemoryManager.infiniteJumpAttack) cheats.Add("J");
            if (MemoryManager.loadTriggers == Enums.CheatStatus.IS_ENABLED) cheats.Add("L");
            if (MemoryManager.otherTriggers == Enums.CheatStatus.IS_ENABLED) cheats.Add("O");
            if (MemoryManager.polyCache == Enums.CheatStatus.IS_ENABLED) cheats.Add("P");
            if (MemoryManager.autoResetSigns) cheats.Add("S");

            if (cheats.Count > 0)
            {
                status += "\nCheats: ";
            }

            for (int i = 0; i < cheats.Count; i++)
            {
                status += cheats[i];

                if (i < cheats.Count - 1)
                {
                    status += " ";
                }
            }

            if (SaveManager.isEnabled)
            {
                status += $"\nSave: {SaveManager.selectedSaveCollectionIndex + 1}-{SaveManager.selectedSaveIndex + 1}";
            }

            status = status.PadRight(65, ' ').Substring(0, 65);

            return status;
        }
    }
}
