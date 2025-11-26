using Memory;
using System.Globalization;
using System.Numerics;

namespace HobbitSpeedrunTools
{
    public class LockClipwarp : ToggleCheat
    {
        public override string Name { get; set; } = "Lock Clipwarp";
        public override string ShortName { get; set; } = "CLIP";
        public override string ShortcutName { get; set; } = "lock_clipwarp";
        public override string ToolTip { get; set; } = "Keeps Bilbo's clipwarp location locked to what it is when this cheat is enabled.";

        public Vector3 SavedWarpPos { get; private set; }

        public LockClipwarp(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnEnable()
        {
            if (!CheatManager.IsHooked || mem == null) return;

            float x = mem.ReadFloat(MemoryAddresses.warpCoordsX);
            float y = mem.ReadFloat(MemoryAddresses.warpCoordsY);
            float z = mem.ReadFloat(MemoryAddresses.warpCoordsZ);

            SavedWarpPos = new(x, y, z);

            base.OnEnable();
        }

        public override void OnTick()
        {
            if (mem == null || !Enabled) return;

            mem.WriteMemory(MemoryAddresses.warpCoordsX, "float", SavedWarpPos.X.ToString(CultureInfo.InvariantCulture));
            mem.WriteMemory(MemoryAddresses.warpCoordsY, "float", SavedWarpPos.Y.ToString(CultureInfo.InvariantCulture));
            mem.WriteMemory(MemoryAddresses.warpCoordsZ, "float", SavedWarpPos.Z.ToString(CultureInfo.InvariantCulture));
        }

        public void SetWarpPosition(Vector3 position)
        {
            SavedWarpPos = position;
        }
    }
}
