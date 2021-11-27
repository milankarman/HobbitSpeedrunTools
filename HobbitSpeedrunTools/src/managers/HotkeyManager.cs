using System;
using NonInvasiveKeyboardHookLibrary;

namespace HobbitSpeedrunTools
{
    public static class HotkeyManager
    {
        private static readonly KeyboardHookManager keyboardHookManager = new();

        public static void InitHotkeyManager()
        {
            keyboardHookManager.Start();

            // Parses and sets the modifier key set in the config
            ModifierKeys modifierKey = ConfigManager.ModifierKey switch
            {
                "ctrl" => ModifierKeys.Control,
                "shift" => ModifierKeys.Shift,
                "alt" => ModifierKeys.Alt,
                "win" => ModifierKeys.WindowsKey,
                _ => throw new Exception("Invalid modifier key"),
            };

            // Binds all methods to their hotkeys
            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShDevMode, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleDevMode(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShInfiniteJumpAttack, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleInfiniteJumpAttacks(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShRenderLoadTriggers, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleRenderLoadTriggers(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShRenderOtherTriggers, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleRenderOtherTriggers(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShRenderPolyCache, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.TogglePolycache(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShAutoResetSigns, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleAutoResetSigns(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShInvincibility, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleInvincibility(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShResetLevel, () =>
            {
                MemoryManager.ResetLevel();
            });

            keyboardHookManager.RegisterHotkey(modifierKey, ConfigManager.ShToggleSaveManager, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleSaveManager());
            });

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
