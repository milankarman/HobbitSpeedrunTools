using System.Threading;
using System.Collections.Generic;
using Memory;
using HobbitSpeedrunTools.cheats;

namespace HobbitSpeedrunTools
{
    public static class MemoryManager
    {
        static List<ToggleCheat> toggleCheats = new()
        {
            new AutoResetSigns(),
            new DevMode(),
            new InfiniteJumpAttack(),
            new Invincibility(),
            new LockClipwarp(),
            new RenderLoadTriggers(),
            new RenderOtherTriggers(),
            new RenderPolyCache(),
        };

        private static readonly Mem mem = new();

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

            //if (DevMode) cheats.Add("DEV");
            //if (InfiniteJumpAttack) cheats.Add("IJA");
            //if (LoadTriggers) cheats.Add("LTRIG");
            //if (OtherTriggers) cheats.Add("OTRIG");
            //if (PolyCache) cheats.Add("POLY");
            //if (AutoResetSigns) cheats.Add("SIGN");
            //if (Invincibility) cheats.Add("INV");
            //if (LockClipwarp) cheats.Add("CLIP");

            return cheats;
        }
    }
}
