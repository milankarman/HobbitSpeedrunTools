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

            // Modifiers are supported too
            keyboardHookManager.RegisterHotkey(ModifierKeys.Control, 0x70, () =>
            {
                MainWindow.Instance?.Test();
            });
        }
    }
}
