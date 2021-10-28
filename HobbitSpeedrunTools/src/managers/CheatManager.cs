using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Memory;

namespace HobbitSpeedrunTools
{
    public static class CheatManager
    {
        private static CancellationTokenSource? cts;
        private static CancellationToken ct;

        private static Mem mem = new Mem();

        public static void InitCheatManager()
        {
            Thread keepHookedThread = new Thread(KeepHooked);
            keepHookedThread.IsBackground = true;
            keepHookedThread.Start();
        }

        private static void KeepHooked()
        {
            while (true)
            {
                mem.OpenProcess("meridian");
                Thread.Sleep(1000);
            }
        }

        private static void InfiniteJumpAttack()
        {
            while (!ct.IsCancellationRequested)
            {
                mem.WriteMemory(MemoryAddresses.stamina, "float", "10");
                Thread.Sleep(100);
            }
        }

        public static void EnableDevMode()
        {
            mem.WriteMemory(MemoryAddresses.devMode, "int", "1");
        }

        public static void DisableDevMode()
        {
            mem.WriteMemory(MemoryAddresses.devMode, "int", "0");
        }

        public static void EnableInfiniteJumpAttack()
        {
            cts = new CancellationTokenSource();
            ct = cts.Token;
            Thread infiniteJumpAttackThread = new Thread(InfiniteJumpAttack);
            infiniteJumpAttackThread.IsBackground = true;
            infiniteJumpAttackThread.Start();
        }

        public static void DisableInfiniteJumpAttack()
        {
            cts?.Cancel();
        }

        public static void EnableLoadTriggers()
        {
            mem.WriteMemory(MemoryAddresses.loadingTriggers, "int", "1");
        }

        public static void DisableLoadTriggers()
        {
            mem.WriteMemory(MemoryAddresses.loadingTriggers, "int", "0");
        }

        public static void EnableOtherTriggers()
        {
            mem.WriteMemory(MemoryAddresses.otherTriggers, "int", "1");
        }

        public static void DisableOtherTriggers()
        {
            mem.WriteMemory(MemoryAddresses.otherTriggers, "int", "0");
        }
    }
}
