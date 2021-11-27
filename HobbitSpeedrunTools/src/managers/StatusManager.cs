using System.Collections.Generic;

namespace HobbitSpeedrunTools
{
    public static class StatusManager
    {
        public static string GetStatusText()
        {
            string status = $"HST {About.version}";

            List<string> cheats = new List<string>();

            if (MemoryManager.devMode == Enums.CheatStatus.IS_ENABLED) cheats.Add("DEV");
            if (MemoryManager.infiniteJumpAttack) cheats.Add("IJA");
            if (MemoryManager.loadTriggers == Enums.CheatStatus.IS_ENABLED) cheats.Add("LTRIG");
            if (MemoryManager.otherTriggers == Enums.CheatStatus.IS_ENABLED) cheats.Add("OTRIG");
            if (MemoryManager.polyCache == Enums.CheatStatus.IS_ENABLED) cheats.Add("POLY");
            if (MemoryManager.autoResetSigns) cheats.Add("SIGN");
            if (MemoryManager.invincibility == Enums.CheatStatus.IS_ENABLED) cheats.Add("INV");

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
