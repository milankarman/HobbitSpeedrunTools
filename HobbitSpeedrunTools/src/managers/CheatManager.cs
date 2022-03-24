using Memory;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace HobbitSpeedrunTools
{
    public enum ACTION_CHEAT_ID
    {
        QUICK_LOAD,
        LEVEL_RELOAD,
    }

    public static class CheatManager
    {
        public static readonly Mem mem = new();

        public static List<ToggleCheat?> toggleCheatList = new();
        public static List<ActionCheat?> actionCheatList = new();

        // Starts a new thread handling the cheat loop
        public static void InitCheatManager()
        {
            // Automatically creates instances of every cheat class inheriting from ToggleCheat
            Assembly? toggleCheatAssembly = Assembly.GetAssembly(typeof(ToggleCheat));

            if (toggleCheatAssembly != null)
            {
                Type[] toggleCheatTypes = toggleCheatAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ToggleCheat))).ToArray();

                foreach (Type type in toggleCheatTypes)
                {
                    ToggleCheat? cheat = (ToggleCheat?)Activator.CreateInstance(type, mem);
                    toggleCheatList.Add(cheat);
                }

                toggleCheatList = toggleCheatList.OrderBy(cheat => cheat?.Index).ToList();
            }

            // Automatically creates instances of every cheat class inheriting from ActionCheat
            Assembly? actionCheatAssembly = Assembly.GetAssembly(typeof(ActionCheat));

            if (actionCheatAssembly != null)
            {
                Type[] actionCheatTypes = actionCheatAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ActionCheat))).ToArray();

                foreach (Type type in actionCheatTypes)
                {
                    actionCheatList.Add((ActionCheat?)Activator.CreateInstance(type, mem));
                }

                actionCheatList = actionCheatList.OrderBy(cheat => cheat?.Index).ToList();
            }

            Thread cheatLoopThread = new(CheatLoop);
            cheatLoopThread.IsBackground = true;
            cheatLoopThread.Start();
        }

        // Continuously checks which cheats should be enabled and handles automatic cheats
        private static void CheatLoop()
        {
            while (true)
            {
                // Attempt to hook to the game's process
                if (mem.OpenProcess("meridian"))
                {
                    mem.WriteMemory(MemoryAddresses.memUsage, "int", "1");
                    mem.WriteMemory(MemoryAddresses.memUsageText, "string", StatusManager.GetStatusText());

                    // Execute every toggled cheat
                    foreach (ToggleCheat? cheat in toggleCheatList)
                    {
                        cheat?.OnTick();
                    }
                }

                // Wait for 100ms before repeating
                Thread.Sleep(100);
            }
        }

        public static List<ToggleCheat?> GetToggleCheats() => toggleCheatList;

        public static List<ActionCheat?> GetActionCheats() => actionCheatList;

        // Gets a list of active cheats with short names
        public static List<string> GetToggleCheatList()
        {
            List<string> cheats = new();

            foreach (ToggleCheat? cheat in toggleCheatList)
            {
                if (cheat != null && cheat.Enabled) cheats.Add(cheat.ShortName ?? "NONAME");
            }

            return cheats;
        }
    }
}
