using Memory;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace HobbitSpeedrunTools
{
    public class CheatManager
    {
        public readonly Mem mem = new();
        public static bool IsHooked { get; private set; } = false;
        public StatusManager? statusManager;

        public readonly ToggleCheat[] toggleCheatList;
        public readonly ActionCheat[] actionCheatList;

        public Action<Vector3>? onBilboPositionUpdate;
        public Action<double>? onBilboRotationUpdate;
        public Action<Vector3>? onClipwarpPositionUpdate;
        public Action<float>? onHealthUpdate;
        public Action<float>? onLevelUpdate;

        public bool lockHealth;
        public float lockedHealth;

        public bool lockLevel;
        public float lockedLevel;

        private LoopLevel loopLevelCheat;

        // Starts a new thread handling the cheat loop
        public CheatManager()
        {
            Thread cheatLoopThread = new(CheatLoop)
            {
                IsBackground = true
            };

            cheatLoopThread.Start();

            loopLevelCheat = new(mem);

            actionCheatList =
            [
                new QuickLoad(mem),
                new LevelReload(mem, loopLevelCheat),
                new InstantClipwarp(mem),
            ];

            toggleCheatList =
            [
                new DevMode(mem),
                new InfiniteJumpAttack(mem),
                new RenderLoadTriggers(mem),
                new RenderOtherTriggers(mem),
                new RenderPolyCache(mem),
                new Invincibility(mem),
                new LockClipwarp(mem),
                new ReloadOnLostWarp(mem),
                new AutoResetSigns(mem),
                loopLevelCheat,
                new LoadScreenOnVendor(mem),
                new InfiniteRing(mem),
                new InfiniteRocks(mem),
                new InfiniteKeys(mem),
                new InfinitePotions(mem),
            ];
        }

        // Continuously checks which cheats should be enabled and handles automatic cheats
        private void CheatLoop()
        {
            while (true)
            {
                if (mem == null) return;

                IsHooked = mem.OpenProcess("meridian.exe");
                // Attempt to hook to the game's process
                if (IsHooked)
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
                    UpdateHealth();
                    UpdateLevel();

                    if (lockHealth) SetHealth(lockedHealth);
                    if (lockLevel) SetLevel(lockedLevel);
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

            onBilboPositionUpdate?.Invoke(new Vector3(x, y, z));
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

            Vector3 warpPosition = new(x, y, z);

            onClipwarpPositionUpdate?.Invoke(warpPosition);
        }

        public void UpdateClipwarpPosition(WarpToggleCheat warpToggleCheat, Vector3 position)
        {
            warpToggleCheat.SetWarpPosition(position);
            onClipwarpPositionUpdate?.Invoke(position);
        }

        public void UpdateHealth()
        {
            float health = mem.ReadFloat(MemoryAddresses.health);
            onHealthUpdate?.Invoke(health);
        }
        public void UpdateLevel()
        {
            float level = mem.ReadFloat(MemoryAddresses.level);
            onLevelUpdate?.Invoke(level);
        }

        public void SetHealth(float health)
        {
            mem.WriteMemory(MemoryAddresses.health, "float", health.ToString());
        }

        public void SetLevel(float level)
        {
            mem.WriteMemory(MemoryAddresses.level, "float", level.ToString());
        }

        public void SetPositionX(float x)
        {
            mem.WriteMemory(MemoryAddresses.bilboCoordsX, "float", x.ToString());
            mem.WriteMemory(MemoryAddresses.bilboNewCoordsX, "float", x.ToString());
        }

        public void SetPositionY(float y)
        {
            mem.WriteMemory(MemoryAddresses.bilboCoordsY, "float", y.ToString());
            mem.WriteMemory(MemoryAddresses.bilboNewCoordsY, "float", y.ToString());
        }

        public void SetPositionZ(float z)
        {
            mem.WriteMemory(MemoryAddresses.bilboCoordsZ, "float", z.ToString());
            mem.WriteMemory(MemoryAddresses.bilboNewCoordsZ, "float", z.ToString());
        }

        public void SetClipwarpPositionX(float x)
        {
            mem.WriteMemory(MemoryAddresses.warpCoordsX, "float", x.ToString());
        }

        public void SetClipwarpPositionY(float y)
        {
            mem.WriteMemory(MemoryAddresses.warpCoordsY, "float", y.ToString());
        }

        public void SetClipwarpPositionZ(float z)
        {
            mem.WriteMemory(MemoryAddresses.warpCoordsZ, "float", z.ToString());
        }

        public void UpdateCheats(SaveManager.SaveSettings settings)
        {
            for (int i = 0; i < toggleCheatList.Length; i++)
            {
                if (toggleCheatList[i] != null)
                {
                    toggleCheatList[i].SetActive(settings.toggles[i]);
                    // Currently reload on lost clipwarp takes precedence over locked clipwarp
                    // But both shouldn't be enabled at the same time?
                    // Something to take a look at to adjust behavior.
                    if (toggleCheatList[i] is WarpToggleCheat warpToggleCheat)
                        UpdateClipwarpPosition(warpToggleCheat, new Vector3(settings.clipwarpX, settings.clipwarpY, settings.clipwarpZ));

                    if (toggleCheatList[i] is LoopLevel loopLevel)
                        loopLevelCheat.loopLevelId = settings.loopLevelId;
                }
            }
        }

        // Gets a list of active cheats with short names
        public List<string> GetToggleCheatList()
        {
            List<string> cheats = [];

            foreach (ToggleCheat? cheat in toggleCheatList)
                if (cheat != null && cheat.Enabled) cheats.Add(cheat.ShortName ?? "NONAME");

            return cheats;
        }
    }
}
