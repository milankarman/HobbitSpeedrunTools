using NonInvasiveKeyboardHookLibrary;

namespace HobbitSpeedrunTools
{
    public static class HotkeyManager
    {
        public static KeyboardHookManager keyboardHookManager = new KeyboardHookManager();

        public static void InitHotkeyManager()
        {
            keyboardHookManager.Start();

            ModifierKeys[] modifierKeys = new ModifierKeys[] { ModifierKeys.Alt };

            keyboardHookManager.RegisterHotkey(modifierKeys, 49, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleDevMode(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 50, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleInfiniteJumpAttacks(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 51, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleRenderLoadTriggers(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 52, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleRenderOtherTriggers(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 53, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.TogglePolycache(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 54, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleAutoResetSigns(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 82, () =>
            {
                MemoryManager.ResetLevel();
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 187, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.NextSave());
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 189, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.PreviousSave());
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 48, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.NextSaveCollection());
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 57, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.PreviousSaveCollection());
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 56, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleSaveManager());
            });
        }
    }
}
