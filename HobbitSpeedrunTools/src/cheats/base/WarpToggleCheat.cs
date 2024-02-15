namespace HobbitSpeedrunTools
{
    public abstract class WarpToggleCheat : ToggleCheat
    {
        public float SavedWarpPosX { get; private set; }
        public float SavedWarpPosY { get; private set; }
        public float SavedWarpPosZ { get; private set; }

        public override void OnEnable()
        {
            if (!CheatManager.isHooked || mem == null) return;

            SavedWarpPosX = mem.ReadFloat(MemoryAddresses.warpCoordsX);
            SavedWarpPosY = mem.ReadFloat(MemoryAddresses.warpCoordsY);
            SavedWarpPosZ = mem.ReadFloat(MemoryAddresses.warpCoordsZ);

            base.OnEnable();
        }

        public virtual void OverwriteSavedWarpPosition(float x, float y, float z)
        {
            SavedWarpPosX = x;
            SavedWarpPosY = y;
            SavedWarpPosZ = z;
        }
    }
}
