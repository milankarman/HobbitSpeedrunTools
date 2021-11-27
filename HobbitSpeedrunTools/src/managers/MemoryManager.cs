using System;
using System.Threading;
using Memory;

namespace HobbitSpeedrunTools
{
    public static class MemoryManager
    {
        private static Mem mem = new Mem();

        // Tracking the status of all cheats
        public static bool infiniteJumpAttack = false;
        public static bool autoResetSigns = false;

        public static Enums.CheatStatus devMode = Enums.CheatStatus.IS_DISABLED;
        public static Enums.CheatStatus loadTriggers = Enums.CheatStatus.IS_DISABLED;
        public static Enums.CheatStatus otherTriggers = Enums.CheatStatus.IS_DISABLED;
        public static Enums.CheatStatus polyCache = Enums.CheatStatus.IS_DISABLED;
        public static Enums.CheatStatus invincibility = Enums.CheatStatus.IS_DISABLED;

        // Starts a new thread handling the cheat loop
        public static void InitMemoryManager()
        {
            Thread cheatLoopThread = new Thread(CheatLoop);
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
                    // Always show the memory usage overlay to use for HSR status
                    mem.WriteMemory(MemoryAddresses.memUsage, "int", "1");
                    mem.WriteMemory(MemoryAddresses.memUsageText, "string", StatusManager.GetStatusText());

                    if (infiniteJumpAttack) mem.WriteMemory(MemoryAddresses.stamina, "float", "10");
                    if (autoResetSigns) ResetSigns();

                    ToggleAddress(ref devMode, MemoryAddresses.devMode);
                    ToggleAddress(ref loadTriggers, MemoryAddresses.loadTriggers);
                    ToggleAddress(ref otherTriggers, MemoryAddresses.otherTriggers);
                    ToggleAddress(ref polyCache, MemoryAddresses.polyCache);
                    ToggleAddress(ref invincibility, MemoryAddresses.invincibility);
                }

                // Wait for 100ms before repeating
                Thread.Sleep(100);
            }
        }

        private static void ToggleAddress(ref Enums.CheatStatus status, string address)
        {
            if (status == Enums.CheatStatus.ENABLE)
            {
                mem.WriteMemory(address, "int", "1");
                status = Enums.CheatStatus.IS_ENABLED;
            }
            else if (status == Enums.CheatStatus.DISABLE)
            {
                mem.WriteMemory(address, "int", "0");
                status = Enums.CheatStatus.IS_DISABLED;
            }
        }

        private static void ResetSigns()
        {
            // Reset the signs if the player loads or dies
            if (mem.ReadInt(MemoryAddresses.loading) == 1 || mem.ReadFloat(MemoryAddresses.health) <= 0)
            {
                mem.WriteMemory(MemoryAddresses.sign1, "int", "1");
                mem.WriteMemory(MemoryAddresses.sign2, "int", "0");
                mem.WriteMemory(MemoryAddresses.sign3, "int", "0");
                mem.WriteMemory(MemoryAddresses.sign4, "int", "1");
                mem.WriteMemory(MemoryAddresses.sign5, "int", "0");
            }
        }

        public static void ResetLevel()
        {
            int currentLevelID = mem.ReadInt(MemoryAddresses.currentLevelID);

            if (currentLevelID >= 0)
            {
                mem.WriteMemory(MemoryAddresses.currentLevelID, "int", (currentLevelID - 1).ToString());
                mem.WriteMemory(MemoryAddresses.load, "int", "1");
            }
        }
    }
}
