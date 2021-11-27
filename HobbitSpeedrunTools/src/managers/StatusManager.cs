using System.Collections.Generic;

namespace HobbitSpeedrunTools
{
    public static class StatusManager
    {
        public static string GetStatusText()
        {
            string status = $"HST {About.version}";

            List<string> cheats = MemoryManager.GetCheatList();


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

            if (SaveManager.IsEnabled)
            {
                status += $"\nSave: {SaveManager.SelectedSaveCollectionIndex + 1}-{SaveManager.SelectedSaveIndex + 1}";
            }

            status = status.PadRight(65, ' ')[..65];

            return status;
        }
    }
}
