using System;
using NonInvasiveKeyboardHookLibrary;

namespace HobbitSpeedrunTools
{
    public static class HotkeyManager
    {
        public static KeyboardHookManager keyboardHookManager = new KeyboardHookManager();

        public static void InitHotkeyManager()
        {
            keyboardHookManager.Start();

            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, 0x70, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleDevMode(true));
            });

            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, 0x71, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleInfiniteJumpAttacks(true));
            });

            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, 0x72, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleRenderLoadTriggers(true));
            });

            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, 0x73, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleRenderOtherTriggers(true));
            });

            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, 0x74, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.TogglePolycache(true));
            });

            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, 0x75, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleAutoResetSigns(true));
            });
        }
    }
}
