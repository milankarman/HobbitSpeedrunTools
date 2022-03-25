using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace HobbitSpeedrunTools
{
    public static class StatusManager
    {
        // Prints the current status of cheats and save manager in 64 characters
        // to fit into the in game display
        public static string GetStatusText()
        {
            string status = $"HST {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion}";

            List<string> cheats = CheatManager.GetToggleCheatList();

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

            if (SaveManager.SelectedSaveCollectionIndex > 0)
            {
                status += $"\nSave: {SaveManager.SelectedSaveCollectionIndex}-{SaveManager.SelectedSaveIndex + 1}";
            }

            status = status.PadRight(65, ' ')[..65];

            return status;
        }
    }
}
