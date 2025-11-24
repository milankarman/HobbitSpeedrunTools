using Memory;
using System.Threading.Tasks;

namespace HobbitSpeedrunTools
{
    public class LevelReload : ActionCheat
    {
        public override string Name { get; set; } = "Level Reload";
        public override string ShortcutName { get; set; } = "level_reload";

        private readonly LoopLevel? loopLevel;

        public LevelReload(Mem _mem, LoopLevel? _loopLevel)
        {
            mem = _mem;
            loopLevel = _loopLevel;
        }

        public async override void Start()
        {
            if (mem == null) return;

            int currentLevelID;

            // If the LoopLevel cheat is enabled load into the selected level
            if (loopLevel != null && loopLevel.Enabled)
            {
                currentLevelID = loopLevel.loopLevelId;
            }
            else
            {
                currentLevelID = mem.ReadInt(MemoryAddresses.currentLevelID);
            }

            if (currentLevelID >= 0)
            {
                mem.WriteMemory(MemoryAddresses.currentLevelID, "int", (currentLevelID - 1).ToString());
                mem.WriteMemory(MemoryAddresses.load, "int", "1");

                if (MainWindow.quickReload)
                {
                    // Try this for 2 seconds (10 ms * 200)
                    for (int i = 0; i < 200; i++)
                    {
                        int OoLState = mem.ReadInt(MemoryAddresses.outOfLevelState);

                        // If we're watching the end level cutscene force the state to gameplay
                        if (OoLState == 17)
                        {
                            mem.WriteMemory(MemoryAddresses.outOfLevelState, "int", "19");
                            break;
                        }

                        // If we're in either vendor states force update it to state 20
                        if (OoLState == 12 || OoLState == 14)
                        {
                            mem.WriteMemory(MemoryAddresses.outOfLevelState, "int", "20");
                        }

                        await Task.Delay(10);
                    }
                }
            }
        }
    }
}
