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

                    if (infiniteJumpAttack)
                    {
                        // Keep stamina at max for infinite jump attacks
                        mem.WriteMemory(MemoryAddresses.stamina, "float", "10");
                    }

                    if (autoResetSigns)
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

                    // Swap the states of cheats and enable them accordingly
                    if (devMode == Enums.CheatStatus.ENABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.devMode, "int", "1");
                        devMode = Enums.CheatStatus.IS_ENABLED;
                    }
                    else if (devMode == Enums.CheatStatus.DISABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.devMode, "int", "0");
                        devMode = Enums.CheatStatus.IS_DISABLED;
                    }

                    if (loadTriggers == Enums.CheatStatus.ENABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.loadTriggers, "int", "1");
                        loadTriggers = Enums.CheatStatus.IS_ENABLED;
                    }
                    else if (loadTriggers == Enums.CheatStatus.DISABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.loadTriggers, "int", "0");
                        loadTriggers = Enums.CheatStatus.IS_DISABLED;
                    }

                    if (otherTriggers == Enums.CheatStatus.ENABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.otherTriggers, "int", "1");
                        otherTriggers = Enums.CheatStatus.IS_ENABLED;
                    }
                    else if (otherTriggers == Enums.CheatStatus.DISABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.otherTriggers, "int", "0");
                        otherTriggers = Enums.CheatStatus.IS_DISABLED;
                    }

                    if (polyCache == Enums.CheatStatus.ENABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.polyCache, "int", "1");
                        polyCache = Enums.CheatStatus.IS_ENABLED;
                    }
                    else if (polyCache == Enums.CheatStatus.DISABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.polyCache, "int", "0");
                        polyCache = Enums.CheatStatus.IS_DISABLED;
                    }
                }
                else
                {
                    //devMode = devMode == Enums.CheatStatus.IS_ENABLED ? Enums.CheatStatus.ENABLE : Enums.CheatStatus.IS_DISABLED;
                    //loadTriggers = loadTriggers == Enums.CheatStatus.IS_ENABLED ? Enums.CheatStatus.ENABLE : Enums.CheatStatus.IS_DISABLED;
                    //otherTriggers = otherTriggers == Enums.CheatStatus.IS_ENABLED ? Enums.CheatStatus.ENABLE : Enums.CheatStatus.IS_DISABLED;
                    //polyCache = polyCache == Enums.CheatStatus.IS_ENABLED ? Enums.CheatStatus.ENABLE : Enums.CheatStatus.IS_DISABLED;
                }

                // Wait for 100ms before repeating
                Thread.Sleep(100);
            }
        }
    }
}
