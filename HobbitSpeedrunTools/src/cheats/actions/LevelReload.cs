using Memory;

namespace HobbitSpeedrunTools
{
    public class LevelReload : ActionCheat
    {
        public override string Name { get; set; } = "Level Reload";
        public override string ShortcutName { get; set; } = "level_reload";

        public LevelReload(Mem _mem)
        {
            mem = _mem;
        }

        public override void Start()
        {
            if (mem == null) return;

            int currentLevelID = mem.ReadInt(MemoryAddresses.currentLevelID);

            if (currentLevelID >= 0)
            {
                mem.WriteMemory(MemoryAddresses.currentLevelID, "int", (currentLevelID - 1).ToString());
                mem.WriteMemory(MemoryAddresses.load, "int", "1");
            }
        }
    }
}
