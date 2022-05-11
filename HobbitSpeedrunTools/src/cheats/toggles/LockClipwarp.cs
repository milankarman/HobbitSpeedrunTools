using Memory;
using System.Globalization;

namespace HobbitSpeedrunTools
{
    public class LockClipwarp : ToggleCheat
    {
        public override string Name { get; set; } = "Lock Clipwarp";
        public override string ShortName { get; set; } = "CLIP";
        public override string ShortcutName { get; set; } = "lock_clipwarp";

        private float savedWarpPosX;
        private float savedWarpPosY;
        private float savedWarpPosZ;

        public LockClipwarp(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnEnable()
        {
            if (mem == null) return;

            savedWarpPosX = mem.ReadFloat(MemoryAddresses.warpCoordsX);
            savedWarpPosY = mem.ReadFloat(MemoryAddresses.warpCoordsY);
            savedWarpPosZ = mem.ReadFloat(MemoryAddresses.warpCoordsZ);

            base.OnEnable();
        }

        public override void OnTick()
        {
            if (Enabled && mem != null)
            {
                mem.WriteMemory(MemoryAddresses.warpCoordsX, "float", savedWarpPosX.ToString(CultureInfo.InvariantCulture));
                mem.WriteMemory(MemoryAddresses.warpCoordsY, "float", savedWarpPosY.ToString(CultureInfo.InvariantCulture));
                mem.WriteMemory(MemoryAddresses.warpCoordsZ, "float", savedWarpPosZ.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
