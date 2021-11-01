using System.Threading;
using Memory;

namespace HobbitSpeedrunTools
{
    public static class CheatManager
    {
        // Enumator to track whether a cheat should be enabled/disabled
        public enum CheatStatus
        {
            ENABLE,
            IS_ENABLED,
            DISABLE,
            IS_DISABLED,
        }

        private static Mem mem = new Mem();

        // Tracking the status of all cheats
        public static bool infiniteJumpAttack = false;
        public static bool autoResetSigns = false;

        public static CheatStatus devMode = CheatStatus.IS_DISABLED;
        public static CheatStatus loadTriggers = CheatStatus.IS_DISABLED;
        public static CheatStatus otherTriggers = CheatStatus.IS_DISABLED;
        public static CheatStatus polyCache = CheatStatus.IS_DISABLED;

        // Starts a new thread handling the cheat loop
        public static void InitCheatManager()
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
                    if (devMode == CheatStatus.ENABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.devMode, "int", "1");
                        devMode = CheatStatus.IS_ENABLED;
                    }
                    else if (devMode == CheatStatus.DISABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.devMode, "int", "0");
                        devMode = CheatStatus.IS_DISABLED;
                    }

                    if (loadTriggers == CheatStatus.ENABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.loadTriggers, "int", "1");
                        devMode = CheatStatus.IS_ENABLED;
                    }
                    else if (loadTriggers == CheatStatus.DISABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.loadTriggers, "int", "0");
                        devMode = CheatStatus.IS_DISABLED;
                    }

                    if (otherTriggers == CheatStatus.ENABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.otherTriggers, "int", "1");
                        devMode = CheatStatus.IS_ENABLED;
                    }
                    else if (otherTriggers == CheatStatus.DISABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.otherTriggers, "int", "0");
                        devMode = CheatStatus.IS_DISABLED;
                    }

                    if (polyCache == CheatStatus.ENABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.polyCache, "int", "1");
                        devMode = CheatStatus.IS_ENABLED;
                    }
                    else if (polyCache == CheatStatus.DISABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.polyCache, "int", "0");
                        devMode = CheatStatus.IS_DISABLED;
                    }
                }
                else
                {
                    // Reset the cheats and checkboxes when the process isn't attached
                    infiniteJumpAttack = false;
                    autoResetSigns = false;
                    devMode = CheatStatus.IS_DISABLED;
                    loadTriggers = CheatStatus.IS_DISABLED;
                    otherTriggers = CheatStatus.IS_DISABLED;
                    polyCache = CheatStatus.IS_DISABLED;

                    MainWindow.Instance?.ResetCheatCheckboxes();
                }

                // Wait for 100ms before repeating
                Thread.Sleep(100);
            }
        }
    }
}
