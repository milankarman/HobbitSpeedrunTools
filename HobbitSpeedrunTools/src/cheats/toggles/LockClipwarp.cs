using Memory;
using System.Globalization;

namespace HobbitSpeedrunTools
{
    public class LockClipwarp : WarpToggleCheat
    {
        public override string Name { get; set; } = "Lock Clipwarp";
        public override string ShortName { get; set; } = "CLIP";
        public override string ShortcutName { get; set; } = "lock_clipwarp";
        public override string ToolTip { get; set; } = "Keeps Bilbo's clipwarp location locked to what it is when this cheat is enabled.";

        public LockClipwarp(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            if (mem == null || !Enabled) return;

            mem.WriteMemory(MemoryAddresses.warpCoordsX, "float", SavedWarpPos.X.ToString(CultureInfo.InvariantCulture));
            mem.WriteMemory(MemoryAddresses.warpCoordsY, "float", SavedWarpPos.Y.ToString(CultureInfo.InvariantCulture));
            mem.WriteMemory(MemoryAddresses.warpCoordsZ, "float", SavedWarpPos.Z.ToString(CultureInfo.InvariantCulture));
        }
    }
}
