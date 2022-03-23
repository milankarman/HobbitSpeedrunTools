using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public class LockClipwarp : ToggleCheat
    {
        public new readonly string shortName = "LOCK";
        public new readonly string shortcutName = "lock_clipwarp";

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
            if (enabled && mem != null)
            {
                mem.WriteMemory(MemoryAddresses.loadTriggers, "int", enabled ? "1" : "0");
                mem.WriteMemory(MemoryAddresses.warpCoordsX, "float", savedWarpPosX.ToString());
                mem.WriteMemory(MemoryAddresses.warpCoordsY, "float", savedWarpPosY.ToString());
                mem.WriteMemory(MemoryAddresses.warpCoordsZ, "float", savedWarpPosZ.ToString());
            }
        }
    }
}
