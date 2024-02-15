using Memory;
using System;
using System.Linq;

namespace HobbitSpeedrunTools
{
    public class ReloadOnLostWarp : ToggleCheat
    {
        public override string Name { get; set; } = "Reload on Lost Warp";
        public override string ShortName { get; set; } = "RELO";
        public override string ShortcutName { get; set; } = "reload_lost_warp";

        private float startX;
        private float startY;
        private float startZ;

        private bool waiting;

        public ReloadOnLostWarp(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            if (mem == null) return;

            if (!Enabled)
            {
                startX = mem.ReadFloat(MemoryAddresses.warpCoordsX);
                startY = mem.ReadFloat(MemoryAddresses.warpCoordsY);
                startZ = mem.ReadFloat(MemoryAddresses.warpCoordsZ);
            }
            else
            {
                if (StateLists.deathStates.Contains(mem.ReadInt(MemoryAddresses.bilboState)))
                {
                    waiting = true;
                }

                if (waiting && StateLists.movementStates.Contains(mem.ReadInt(MemoryAddresses.bilboState)))
                {
                    waiting = false;
                }

                if (waiting)
                {
                    mem.WriteMemory(MemoryAddresses.warpCoordsX, "float", startX.ToString());
                    mem.WriteMemory(MemoryAddresses.warpCoordsY, "float", startY.ToString());
                    mem.WriteMemory(MemoryAddresses.warpCoordsZ, "float", startZ.ToString());

                    return;
                }

                float x = mem.ReadFloat(MemoryAddresses.warpCoordsX);
                float y = mem.ReadFloat(MemoryAddresses.warpCoordsY);
                float z = mem.ReadFloat(MemoryAddresses.warpCoordsZ);

                if (x == 0 && y == 0 && z == 0)
                {
                    return;
                }

                if (startX != x || startY != y || startZ != z)
                {
                    mem?.WriteMemory(MemoryAddresses.stamina, "float", "10");
                    mem?.WriteMemory(MemoryAddresses.bilboState, "int", "27");

                    waiting = true;
                }
            }
        }
    }
}
