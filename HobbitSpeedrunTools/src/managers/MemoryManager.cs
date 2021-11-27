using System.Threading;
using System.Collections.Generic;
using Memory;

namespace HobbitSpeedrunTools
{
    public static class MemoryManager
    {
        public static bool DevMode { get; set; }
        public static bool InfiniteJumpAttack { get; set; }
        public static bool LoadTriggers { get; set; }
        public static bool OtherTriggers { get; set; }
        public static bool PolyCache { get; set; }
        public static bool AutoResetSigns { get; set; }
        public static bool Invincibility { get; set; }

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
                    // Always show the memory usage overlay to use for HSR status
                    mem.WriteMemory(MemoryAddresses.memUsage, "int", "1");
                    mem.WriteMemory(MemoryAddresses.memUsageText, "string", StatusManager.GetStatusText());

                    // Write cheats to memory
                    mem.WriteMemory(MemoryAddresses.devMode, "int", DevMode ? "1" : "0");
                    if (InfiniteJumpAttack) mem.WriteMemory(MemoryAddresses.stamina, "float", "10");
                    mem.WriteMemory(MemoryAddresses.loadTriggers, "int", LoadTriggers ? "1" : "0");
                    mem.WriteMemory(MemoryAddresses.otherTriggers, "int", OtherTriggers ? "1" : "0");
                    mem.WriteMemory(MemoryAddresses.polyCache, "int", PolyCache ? "1" : "0");
                    mem.WriteMemory(MemoryAddresses.invincibility, "int", Invincibility ? "1" : "0");
                    if (AutoResetSigns) ResetSigns();
                }

                // Wait for 100ms before repeating
                Thread.Sleep(100);
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

        public static List<string> GetCheatList()
        {
            List<string> cheats = new();

            if (DevMode) cheats.Add("DEV");
            if (InfiniteJumpAttack) cheats.Add("IJA");
            if (LoadTriggers) cheats.Add("LTRIG");
            if (OtherTriggers) cheats.Add("OTRIG");
            if (PolyCache) cheats.Add("POLY");
            if (AutoResetSigns) cheats.Add("SIGN");
            if (Invincibility) cheats.Add("INV");

            return cheats;
        }
    }
}
