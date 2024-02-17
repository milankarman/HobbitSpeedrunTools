using Memory;
using System.Globalization;

namespace HobbitSpeedrunTools
{
    public class LockClipwarp : WarpToggleCheat
    {
        public override string Name { get; set; } = "Lock Clipwarp";
        public override string ShortName { get; set; } = "CLIP";
        public override string ShortcutName { get; set; } = "lock_clipwarp";

        public LockClipwarp(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            if (mem == null || !Enabled) return;

            mem.WriteMemory(MemoryAddresses.warpCoordsX, "float", SavedWarpPosX.ToString(CultureInfo.InvariantCulture));
            mem.WriteMemory(MemoryAddresses.warpCoordsY, "float", SavedWarpPosY.ToString(CultureInfo.InvariantCulture));
            mem.WriteMemory(MemoryAddresses.warpCoordsZ, "float", SavedWarpPosZ.ToString(CultureInfo.InvariantCulture));
        }
    }
}
