using System.Threading;
using System.Windows.Threading;
using Memory;

namespace HobbitSpeedrunTools
{
    public static class CheatManager
    {
        public enum CheatStatus
        {
            ENABLE,
            IS_ENABLED,
            DISABLE,
            IS_DISABLED,
        }

        private static Mem mem = new Mem();

        public static bool infiniteJumpAttack = false;
        public static bool autoResetSigns = false;

        public static CheatStatus devMode = CheatStatus.IS_DISABLED;
        public static CheatStatus loadTriggers = CheatStatus.IS_DISABLED;
        public static CheatStatus otherTriggers = CheatStatus.IS_DISABLED;
        public static CheatStatus polyCache = CheatStatus.IS_DISABLED;

        public static void InitCheatManager()
        {
            Thread cheatLoopThread = new Thread(CheatLoop);
            cheatLoopThread.IsBackground = true;
            cheatLoopThread.Start();
        }

        private static void CheatLoop()
        {
            while (true)
            {
                if (mem.OpenProcess("meridian"))
                {
                    if (infiniteJumpAttack)
                    {
                        mem.WriteMemory(MemoryAddresses.stamina, "float", "10");
                    }

                    if (autoResetSigns)
                    {
                        if (mem.ReadInt(MemoryAddresses.loading) == 1 || mem.ReadFloat(MemoryAddresses.health) <= 0)
                        {
                            mem.WriteMemory(MemoryAddresses.sign1, "int", "1");
                            mem.WriteMemory(MemoryAddresses.sign2, "int", "0");
                            mem.WriteMemory(MemoryAddresses.sign3, "int", "0");
                            mem.WriteMemory(MemoryAddresses.sign4, "int", "1");
                            mem.WriteMemory(MemoryAddresses.sign5, "int", "0");
                        }
                    }

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
                    else if (otherTriggers == CheatStatus.DISABLE)
                    {
                        mem.WriteMemory(MemoryAddresses.polyCache, "int", "0");
                        devMode = CheatStatus.IS_DISABLED;
                    }
                }
                else
                {
                    infiniteJumpAttack = false;
                    autoResetSigns = false;
                    devMode = CheatStatus.IS_DISABLED;
                    loadTriggers = CheatStatus.IS_DISABLED;
                    otherTriggers = CheatStatus.IS_DISABLED;
                    polyCache = CheatStatus.IS_DISABLED;


                    MainWindow.Instance?.ResetCheatCheckboxes();
                }

                Thread.Sleep(100);
            }
        }
    }
}
