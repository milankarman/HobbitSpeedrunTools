using Memory;
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

        public readonly static ToggleCheat[] toggleCheatList =
        {
            new DevMode(mem),
            new InfiniteJumpAttack(mem),
            new RenderLoadTriggers(mem),
            new RenderOtherTriggers(mem),
            new RenderPolyCache(mem),
            new Invincibility(mem),
            new AutoResetSigns(mem),
            new LockClipwarp(mem),
        };

        public readonly static ActionCheat[] actionCheatList =
        {
            new QuickLoad(mem),
            new LevelReload(mem),
        };

        // Starts a new thread handling the cheat loop
        public static void InitCheatManager()
        {
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
