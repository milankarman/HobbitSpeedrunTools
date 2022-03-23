using System.Threading;
using System.Collections.Generic;
using Memory;
using HobbitSpeedrunTools.cheats;

namespace HobbitSpeedrunTools
{
    public static class MemoryManager
    {
        public static readonly Mem mem = new();

        public static ToggleCheat[] toggleCheats =
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



        // Starts a new thread handling the cheat loop
        public static void InitMemoryManager()
        {
            Thread cheatLoopThread = new(CheatLoop);
            cheatLoopThread.IsBackground = true;
            cheatLoopThread.Start();
        }

        // Repeadetly checks which cheats should be enabled and handles automatic cheats
        private static void CheatLoop()
        {
            while (true)
            {
                // Attempt to hook to the game's process
                if (mem.OpenProcess("meridian"))
                {
                    // Execute every toggled cheat
                    foreach(ToggleCheat cheat in toggleCheats)
                    {
                        cheat.OnTick(mem);
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

            foreach(ToggleCheat cheat in toggleCheats)
            {
                if (cheat.enabled) cheats.Add(cheat.shortName ?? "NONAME");
            }

            return cheats;
        }
    }
}
