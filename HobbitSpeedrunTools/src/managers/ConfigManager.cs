using IniParser;
using IniParser.Model;

namespace HobbitSpeedrunTools
{
    public class ConfigManager
    {
        private readonly FileIniDataParser parser = new();
        private IniData data = new();

        public string? ModifierKey { private set; get; }

        public int ShNextSaveCollection { private set; get; }
        public int ShPreviousSaveCollection { private set; get; }
        public int ShNextSave { private set; get; }
        public int ShPreviousSave { private set; get; }

        public ConfigManager()
        {
            data = parser.ReadFile("./config.ini");

            ModifierKey = data["Shortcuts"]["modifier_key"];

            ShNextSaveCollection = int.Parse(data["Shortcuts"]["next_save_collection"]);
            ShPreviousSaveCollection = int.Parse(data["Shortcuts"]["previous_save_collection"]);
            ShNextSave = int.Parse(data["Shortcuts"]["next_save"]);
            ShPreviousSave = int.Parse(data["Shortcuts"]["previous_save"]);
        }

        public int GetShortcut(string key)
        {
            return int.Parse(data["Shortcuts"][key]);
        }
    }
}
