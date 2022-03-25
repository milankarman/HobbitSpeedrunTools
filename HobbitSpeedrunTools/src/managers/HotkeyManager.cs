using NonInvasiveKeyboardHookLibrary;
using System;

namespace HobbitSpeedrunTools
{
    public static class HotkeyManager
    {
        private static readonly KeyboardHookManager keyboardHookManager = new();
        private static ModifierKeys modifierKey;

        public static void BindCheatShortcuts()
        {
            foreach (ToggleCheat cheat in CheatManager.toggleCheatList)
            {
                if (!string.IsNullOrEmpty(cheat.ShortcutName))
                {
                    keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.GetShortcut(cheat.ShortcutName), () => cheat.Toggle());
                }
            }

            foreach (ActionCheat cheat in CheatManager.actionCheatList)
            {
                if (!string.IsNullOrEmpty(cheat.ShortcutName))
                {
                    keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.GetShortcut(cheat.ShortcutName), () => cheat.Start());
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

            BindCheatShortcuts();

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShNextSaveCollection, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.NextSaveCollection());
            });

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShPreviousSaveCollection, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.PreviousSaveCollection());
            });

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShNextSave, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.NextSave());
            });

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShPreviousSave, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.PreviousSave());
            });
        }
    }
}
