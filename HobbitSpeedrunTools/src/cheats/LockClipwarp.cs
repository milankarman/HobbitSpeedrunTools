using Memory;

namespace HobbitSpeedrunTools.Cheats
{
    public class LockClipwarp : ToggleCheat
    {
        public override CHEAT_ID ID { get; set; } = CHEAT_ID.LOCK_CLIPWARP;
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
                mem.WriteMemory(MemoryAddresses.warpCoordsX, "float", savedWarpPosX.ToString());
                mem.WriteMemory(MemoryAddresses.warpCoordsY, "float", savedWarpPosY.ToString());
                mem.WriteMemory(MemoryAddresses.warpCoordsZ, "float", savedWarpPosZ.ToString());
            }
        }
    }
}
