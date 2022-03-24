using Memory;
using System.Collections.Generic;
using System.Threading;

namespace HobbitSpeedrunTools
{
    public enum TOGGLE_CHEAT_ID
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

    public enum ACTION_CHEAT_ID
    {
        QUICK_RESET,
    }

    public static class CheatManager
    {
        public static readonly Mem mem = new();

        public static readonly ToggleCheat[] toggleCheatList =
        {
            new DevMode(mem),
            new InfiniteJumpAttack(mem),
            new RenderLoadTriggers(mem),
            new RenderOtherTriggers(mem),
            new RenderPolyCache(mem),
            new AutoResetSigns(mem),
            new Invincibility(mem),
            new LockClipwarp(mem),
        };

        public static readonly ActionCheat[] actionCheatList =
        {
            new QuickReset(mem),
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
                    foreach (ToggleCheat cheat in toggleCheatList)
                    {
                        cheat.OnTick();
                    }
                }

                // Wait for 100ms before repeating
                Thread.Sleep(100);
            }
        }

        public static ToggleCheat? GetCheat(TOGGLE_CHEAT_ID id)
        {
            foreach (ToggleCheat cheat in toggleCheatList)
            {
                if (cheat.ID == id) return cheat;
            }

            return null;
        }

        public static ActionCheat? GetCheat(ACTION_CHEAT_ID id)
        {
            foreach (ActionCheat cheat in actionCheatList)
            {
                if (cheat.ID == id) return cheat;
            }

            return null;
        }

        public static ToggleCheat[] GetToggleCheats() => toggleCheatList;

        public static ActionCheat[] GetActionCheats() => actionCheatList;

        // Gets a list of active cheats with short names
        public static List<string> GetToggleCheatList()
        {
            List<string> cheats = new();

            foreach (ToggleCheat cheat in toggleCheatList)
            {
                if (cheat.Enabled) cheats.Add(cheat.ShortName ?? "NONAME");
            }

            return cheats;
        }
    }
}
