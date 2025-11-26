using Memory;

namespace HobbitSpeedrunTools
{
    public class LoadScreenOnVendor : ToggleCheat
    {
        public override string Name { get; set; } = "Load Screen on Vendor";
        public override string ShortName { get; set; } = "LVEND";
        public override string ShortcutName { get; set; } = "load_screen_on_vendor";
        public override string ToolTip { get; set; } = "Ending a level will open the load screen instead of vendor. Good for practicing segments that lead into level end.";

        public LoadScreenOnVendor(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            if (!Enabled) return;

            int OoLState = mem?.ReadInt(MemoryAddresses.outOfLevelState) ?? 0;

            if (OoLState == 12 || OoLState == 14)
            {
                mem?.WriteMemory(MemoryAddresses.outOfLevelState, "int", "15");
            }
        }
    }
}
