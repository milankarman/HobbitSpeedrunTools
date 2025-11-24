using Memory;

namespace HobbitSpeedrunTools
{
    public class QuickLoad : ActionCheat
    {
        public override string Name { get; set; } = "Quick Load";
        public override string ShortcutName { get; set; } = "quick_load";

        public QuickLoad(Mem _mem)
        {
            mem = _mem;
        }

        public override void Start()
        {
            mem?.WriteMemory(MemoryAddresses.stamina, "float", "10");

            if (MainWindow.quickReload)
            {
                mem?.WriteMemory(MemoryAddresses.loading, "int", "1");
            }
            else
            {
                mem?.WriteMemory(MemoryAddresses.bilboState, "int", "27");
            }
        }
    }
}
