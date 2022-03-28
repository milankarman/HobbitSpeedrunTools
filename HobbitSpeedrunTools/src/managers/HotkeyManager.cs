using NonInvasiveKeyboardHookLibrary;
using System;

namespace HobbitSpeedrunTools
{
    public class HotkeyManager
    {
        private readonly KeyboardHookManager keyboardHookManager = new();
        private ModifierKeys modifierKey;

        private SaveManager saveManager;
        private CheatManager cheatManager;
        private ConfigManager configManager;

        public HotkeyManager(SaveManager _saveManager, CheatManager _cheatManager, ConfigManager _configManager)
        {
            saveManager = _saveManager;
            cheatManager = _cheatManager;
            configManager = _configManager;

            keyboardHookManager.Start();

            // Parses and sets the modifier key set in the config
            modifierKey = configManager.ModifierKey switch
            {
                "ctrl" => ModifierKeys.Control,
                "shift" => ModifierKeys.Shift,
                "alt" => ModifierKeys.Alt,
                "win" => ModifierKeys.WindowsKey,
                _ => throw new Exception("Invalid modifier key"),
            };

            BindCheatHotkeys();
            BindSaveHotkeys();
        }

        private void BindSaveHotkeys()
        {
            keyboardHookManager.RegisterHotkey(modifierKey, configManager.ShNextSaveCollection, () => saveManager.NextSaveCollection());
            keyboardHookManager.RegisterHotkey(modifierKey, configManager.ShPreviousSaveCollection, () => saveManager.PreviousSaveCollection());
            keyboardHookManager.RegisterHotkey(modifierKey, configManager.ShNextSave, () => saveManager.NextSave());
            keyboardHookManager.RegisterHotkey(modifierKey, configManager.ShPreviousSave, () => saveManager.PreviousSave());
        }

        public void BindCheatHotkeys()
        {
            foreach (ToggleCheat cheat in cheatManager.toggleCheatList)
            {
                if (!string.IsNullOrEmpty(cheat.ShortcutName))
                {
                    int keyCode = configManager.GetShortcut(cheat.ShortcutName);

                    if (keyCode != 0)
                    {
                        keyboardHookManager.RegisterHotkey(modifierKey, keyCode, () =>
                        {
                            try
                            {
                                if (!cheat.Enabled) cheat.Enable();
                                else cheat.Disable();
                            }
                            catch { }
                        });
                    }
                }
            }

            foreach (ActionCheat cheat in cheatManager.actionCheatList)
            {
                if (!string.IsNullOrEmpty(cheat.ShortcutName))
                {
                    keyboardHookManager.RegisterHotkey(modifierKey, configManager.GetShortcut(cheat.ShortcutName), () =>
                    {
                        try
                        {
                            cheat.Start();
                        }
                        catch { }
                    });
                }
            }
        }
    }
}
