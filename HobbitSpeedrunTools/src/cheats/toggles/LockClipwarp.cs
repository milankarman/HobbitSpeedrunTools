using Memory;
using System.Globalization;

namespace HobbitSpeedrunTools
{
    public class LockClipwarp : ToggleCheat
    {
        public override string Name { get; set; } = "Lock Clipwarp";
        public override string ShortName { get; set; } = "CLIP";
        public override string ShortcutName { get; set; } = "lock_clipwarp";

        public float SavedWarpPosX { get; private set; }
        public float SavedWarpPosY { get; private set; }
        public float SavedWarpPosZ { get; private set; }

        public LockClipwarp(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnEnable()
        {
            if (!CheatManager.isHooked || mem == null) return;

            SavedWarpPosX = mem.ReadFloat(MemoryAddresses.warpCoordsX);
            SavedWarpPosY = mem.ReadFloat(MemoryAddresses.warpCoordsY);
            SavedWarpPosZ = mem.ReadFloat(MemoryAddresses.warpCoordsZ);

            base.OnEnable();
        }

        public override void OnTick()
        {
            if (Enabled && mem != null)
            {
                mem.WriteMemory(MemoryAddresses.warpCoordsX, "float", SavedWarpPosX.ToString(CultureInfo.InvariantCulture));
                mem.WriteMemory(MemoryAddresses.warpCoordsY, "float", SavedWarpPosY.ToString(CultureInfo.InvariantCulture));
                mem.WriteMemory(MemoryAddresses.warpCoordsZ, "float", SavedWarpPosZ.ToString(CultureInfo.InvariantCulture));
            }
        }

        public void OverwriteSavedWarpPosition(float x, float y, float z)
        {
            SavedWarpPosX = x;
            SavedWarpPosY = y;
            SavedWarpPosZ = z;
        }
    }
}
