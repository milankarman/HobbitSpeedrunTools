using IniParser;
using IniParser.Model;

namespace HobbitSpeedrunTools
{
    public static class ConfigManager
    {

        public static string? ModifierKey { private set; get; }

        // Cheats
        public static int ShDevMode { private set; get; }
        public static int ShInfiniteJumpAttack { private set; get; }
        public static int ShRenderLoadTriggers { private set; get; }
        public static int ShRenderOtherTriggers { private set; get; }
        public static int ShRenderPolyCache { private set; get; }
        public static int ShAutoResetSigns { private set; get; }
        public static int ShInvincibility { private set; get; }

        // Other
        public static int ShResetLevel { private set; get; }
        public static int ShToggleSaveManager { private set; get; }
        public static int ShNextSaveCollection { private set; get; }
        public static int ShPreviousSaveCollection { private set; get; }
        public static int ShNextSave { private set; get; }
        public static int ShPreviousSave { private set; get; }


        public static void InitConfigManager()
        {
            FileIniDataParser parser = new();
            IniData data = parser.ReadFile("config.ini");

            ModifierKey = data["Shortcuts"]["modifier_key"];

            ShDevMode = int.Parse(data["Shortcuts"]["dev_mode"]);
            ShInfiniteJumpAttack = int.Parse(data["Shortcuts"]["infinite_jump_attack"]);
            ShRenderLoadTriggers = int.Parse(data["Shortcuts"]["render_load_triggers"]);
            ShRenderOtherTriggers = int.Parse(data["Shortcuts"]["render_other_triggers"]);
            ShRenderPolyCache = int.Parse(data["Shortcuts"]["render_polycache"]);
            ShAutoResetSigns = int.Parse(data["Shortcuts"]["automatically_reset_signs"]);
            ShRenderOtherTriggers = int.Parse(data["Shortcuts"]["render_other_triggers"]);
            ShInvincibility = int.Parse(data["Shortcuts"]["invincibility"]);

            ShResetLevel = int.Parse(data["Shortcuts"]["reset_level"]);
            ShToggleSaveManager = int.Parse(data["Shortcuts"]["toggle_save_manager"]);
            ShNextSaveCollection = int.Parse(data["Shortcuts"]["next_save_collection"]);
            ShPreviousSaveCollection = int.Parse(data["Shortcuts"]["previous_save_collection"]);
            ShNextSave = int.Parse(data["Shortcuts"]["next_save"]);
            ShPreviousSave = int.Parse(data["Shortcuts"]["previous_save"]);
        }
    }
}
