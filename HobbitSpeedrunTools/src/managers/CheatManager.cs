using Memory;
using System.Collections.Generic;
using System.Threading;

namespace HobbitSpeedrunTools
{
    public enum CHEAT_ID
    {
        AUTO_RESET_SIGNS,
        DEV_MODE,
        INFINITE_JUMP_ATTACKS,
        INVINCIBILITY,
        LOCK_CLIPWARP,
        RENDER_LOAD_TRIGGERS,
        RENDER_OTHER_TRIGGERS,
        RENDER_POLY_CACHE,
    }

    public static class CheatManager
    {
        public static readonly Mem mem = new();

        public static readonly ToggleCheat[] cheatList =
        {
            new AutoResetSigns(mem),
            new DevMode(mem),
            new InfiniteJumpAttack(mem),
            new Invincibility(mem),
            new LockClipwarp(mem),
            new RenderLoadTriggers(mem),
            new RenderOtherTriggers(mem),
            new RenderPolyCache(mem),
        };

        public static ToggleCheat? GetCheat(CHEAT_ID id)
        {
            foreach (ToggleCheat cheat in cheatList)
            {
                if (cheat.ID == id) return cheat;
            }

            return null;
        }

        public static ToggleCheat[] GetCheats() => cheatList;

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
                    foreach (ToggleCheat cheat in cheatList)
                    {
                        cheat.OnTick();
                    }
                }

                // Wait for 100ms before repeating
                Thread.Sleep(100);
            }
        }

        // Resets the current level
        public static void QuickReload()
        {
            if (mem.OpenProcess("meridian"))
            {
                mem.WriteMemory(MemoryAddresses.stamina, "float", "10");
                mem.WriteMemory(MemoryAddresses.bilboState, "int", "27");
            }
        }

        // Gets a list of active cheats with short names
        public static List<string> GetCheatList()
        {
            List<string> cheats = new();

            foreach (ToggleCheat cheat in cheatList)
            {
                if (cheat.Enabled) cheats.Add(cheat.ShortName ?? "NONAME");
            }

            return cheats;
        }
    }
}
