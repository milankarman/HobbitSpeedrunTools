using Memory;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HobbitSpeedrunTools
{
    public class CheatManager
    {
        public readonly Mem mem = new();
        public StatusManager? statusManager; 

        public readonly ToggleCheat[] toggleCheatList;
        public readonly ActionCheat[] actionCheatList;

        public Action<float, float, float>? onBilboPositionUpdate;
        public Action<double>? onBilboRotationUpdate;
        public Action<float, float, float>? onClipwarpPositionUpdate;

        // Starts a new thread handling the cheat loop
        public CheatManager()
        {
            Thread cheatLoopThread = new(CheatLoop);
            cheatLoopThread.IsBackground = true;
            cheatLoopThread.Start();

            toggleCheatList = new ToggleCheat[]
            {
                new DevMode(mem),
                new InfiniteJumpAttack(mem),
                new RenderLoadTriggers(mem),
                new RenderOtherTriggers(mem),
                new RenderPolyCache(mem),
                new Invincibility(mem),
                new AutoResetSigns(mem),
                new LockClipwarp(mem),
                new InfiniteRing(mem),
                new InfiniteRocks(mem),
                new InfiniteKeys(mem),
            };

            actionCheatList = new ActionCheat[]
            {
                new QuickLoad(mem),
                new LevelReload(mem),
                new InstantClipwarp(mem),
            };
        }

        // Continuously checks which cheats should be enabled and handles automatic cheats
        private void CheatLoop()
        {
            while (true)
            {
                // Attempt to hook to the game's process
                if (mem.OpenProcess("meridian"))
                {
                    if (statusManager != null)
                    {
                        mem.WriteMemory(MemoryAddresses.memUsage, "int", "1");
                        mem.WriteMemory(MemoryAddresses.memUsageText, "string", statusManager.GetStatusText());
                    }

                    // Execute every toggled cheat
                    foreach (ToggleCheat? cheat in toggleCheatList)
                    {
                        cheat?.OnTick();
                    }

                    UpdateBilboPosition();
                    UpdateBilboRotation();
                    UpdateClipwarpPosition();
                }

                // Wait for 100ms before repeating
                Thread.Sleep(100);
            }
        }

        public void UpdateBilboPosition()
        {
            float x = mem.ReadFloat(MemoryAddresses.bilboCoordsX);
            float y = mem.ReadFloat(MemoryAddresses.bilboCoordsY);
            float z = mem.ReadFloat(MemoryAddresses.bilboCoordsZ);

            onBilboPositionUpdate?.Invoke(x, y, z);
        }

        public void UpdateBilboRotation()
        {
            float radians = mem.ReadFloat(MemoryAddresses.bilboYawRad);

            double degrees = 0;

            if (radians != 0)
            {
                degrees = (180 / Math.PI) * radians;
                degrees = 360 - degrees;
            }

            onBilboRotationUpdate?.Invoke(degrees);
        }

        public void UpdateClipwarpPosition()
        {
            float x = mem.ReadFloat(MemoryAddresses.warpCoordsX);
            float y = mem.ReadFloat(MemoryAddresses.warpCoordsY);
            float z = mem.ReadFloat(MemoryAddresses.warpCoordsZ);

            onClipwarpPositionUpdate?.Invoke(x, y, z);
        }

        // Gets a list of active cheats with short names
        public List<string> GetToggleCheatList()
        {
            List<string> cheats = new();

            foreach (ToggleCheat? cheat in toggleCheatList)
            {
                if (cheat != null && cheat.Enabled) cheats.Add(cheat.ShortName ?? "NONAME");
            }

            return cheats;
        }
    }
}
