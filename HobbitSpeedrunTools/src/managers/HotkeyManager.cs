using NonInvasiveKeyboardHookLibrary;

namespace HobbitSpeedrunTools
{
    public static class HotkeyManager
    {
        private static readonly KeyboardHookManager keyboardHookManager = new();

        public static void InitHotkeyManager()
        {
            keyboardHookManager.Start();

            ModifierKeys[] modifierKeys = new ModifierKeys[] { ModifierKeys.Alt };

            keyboardHookManager.RegisterHotkey(modifierKeys, ConfigManager.ShDevMode, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleDevMode(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, ConfigManager.ShInfiniteJumpAttack, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleInfiniteJumpAttacks(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, ConfigManager.ShRenderLoadTriggers, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleRenderLoadTriggers(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, ConfigManager.ShRenderOtherTriggers, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleRenderOtherTriggers(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, ConfigManager.ShRenderPolyCache, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.TogglePolycache(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, ConfigManager.ShAutoResetSigns, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleAutoResetSigns(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, ConfigManager.ShResetLevel, () =>
            {
                MemoryManager.ResetLevel();
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, ConfigManager.ShToggleSaveManager, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleSaveManager());
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, ConfigManager.ShNextSaveCollection, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.NextSaveCollection());
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, ConfigManager.ShPreviousSaveCollection, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.PreviousSaveCollection());
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, ConfigManager.ShNextSave, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.NextSave());
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, ConfigManager.ShPreviousSave, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.PreviousSave());
            });
        }
    }
}
