using IniParser;
using IniParser.Model;

namespace HobbitSpeedrunTools
{
    public static class ConfigManager
    {
        public static string ModifierKey { private set; get; }

        public static int ShQuickReload { private set; get; }
        public static int ShToggleSaveManager { private set; get; }
        public static int ShNextSaveCollection { private set; get; }
        public static int ShPreviousSaveCollection { private set; get; }
        public static int ShNextSave { private set; get; }
        public static int ShPreviousSave { private set; get; }

        private static FileIniDataParser parser = new();
        private static IniData data = new();

        public static int GetShortcut(string key)
        {
            return int.Parse(data["Shortcuts"][key]);
        }

        public static void InitConfigManager()
        {
            data = parser.ReadFile("config.ini");

            ModifierKey = data["Shortcuts"]["modifier_key"];

            ShQuickReload = int.Parse(data["Shortcuts"]["quick_reload"]);
            ShToggleSaveManager = int.Parse(data["Shortcuts"]["toggle_save_manager"]);
            ShNextSaveCollection = int.Parse(data["Shortcuts"]["next_save_collection"]);
            ShPreviousSaveCollection = int.Parse(data["Shortcuts"]["previous_save_collection"]);
            ShNextSave = int.Parse(data["Shortcuts"]["next_save"]);
            ShPreviousSave = int.Parse(data["Shortcuts"]["previous_save"]);
        }
    }
}
