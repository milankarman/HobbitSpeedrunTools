using System;
using NonInvasiveKeyboardHookLibrary;

namespace HobbitSpeedrunTools
{
    public static class HotkeyManager
    {
        private static readonly KeyboardHookManager keyboardHookManager = new();
        private static ModifierKeys modifierKey;

        public static void BindCheatShortcuts(ToggleCheat[] cheats)
        {
            foreach (ToggleCheat cheat in cheats)
            {
                if (!string.IsNullOrEmpty(cheat.ShortcutName))
                {
                    keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.GetShortcut(cheat.ShortcutName), () =>
                    {
                        cheat.Toggle();
                    });
                }
            }
        }

        public static void InitHotkeyManager()
        {
            keyboardHookManager.Start();

            // Parses and sets the modifier key set in the config
            modifierKey = ConfigManager.ModifierKey switch
            {
                "ctrl" => ModifierKeys.Control,
                "shift" => ModifierKeys.Shift,
                "alt" => ModifierKeys.Alt,
                "win" => ModifierKeys.WindowsKey,
                _ => throw new Exception("Invalid modifier key"),
            };

            BindCheatShortcuts(CheatManager.GetCheats());

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShQuickReload, () =>
            {
                CheatManager.QuickReload();
            });

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShToggleSaveManager, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleSaveManager());
            });
        }
    }
}
