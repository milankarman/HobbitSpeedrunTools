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

            ModifierKeys[] modifierKeys = new ModifierKeys[] { ModifierKeys.Control };

            keyboardHookManager.RegisterHotkey(modifierKeys, 68, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleDevMode(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 73, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleInfiniteJumpAttacks(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 76, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleRenderLoadTriggers(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 79, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleRenderOtherTriggers(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 80, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.TogglePolycache(true));
            });

            keyboardHookManager.RegisterHotkey(modifierKeys, 65, () =>
            {
                MainWindow.Instance?.Dispatcher.Invoke(() => MainWindow.Instance.ToggleAutoResetSigns(true));
            });
        }
    }
}
