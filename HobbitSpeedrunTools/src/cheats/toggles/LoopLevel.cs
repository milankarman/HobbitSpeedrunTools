using Memory;

namespace HobbitSpeedrunTools
{
    public class LoopLevel : ToggleCheat
    {
        public override string Name { get; set; } = "Loop Current Level";
        public override string ShortName { get; set; } = "LOOP";
        public override string ShortcutName { get; set; } = "loop_level";
        public override string ToolTip { get; set; } = "Ending a level with attempt to load the level that was active when this is activated.";

        public int loopLevelId;
        bool shouldUpdateLevelID;

        public LoopLevel(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnEnable()
        {
            int? _loopLevelId = mem?.ReadInt(MemoryAddresses.currentLevelID);

            if (_loopLevelId == null)
            {
                Disable();
                return;
            }

            loopLevelId = (int)_loopLevelId;

            shouldUpdateLevelID = true;
        }

        public override void OnDisable()
        {
            mem?.WriteMemory(MemoryAddresses.currentLevelID, "int", loopLevelId.ToString());
        }

        public override void OnTick()
        {
            if (!Enabled) return;

            if (mem?.ReadInt(MemoryAddresses.loadFinished) == 1)
            {
                shouldUpdateLevelID = true;
            }

            if (shouldUpdateLevelID && mem?.ReadInt(MemoryAddresses.outOfLevelState) == 19)
            {
                mem?.WriteMemory(MemoryAddresses.currentLevelID, "int", (loopLevelId - 1).ToString());
                shouldUpdateLevelID = false;
            }
        }
    }
}
