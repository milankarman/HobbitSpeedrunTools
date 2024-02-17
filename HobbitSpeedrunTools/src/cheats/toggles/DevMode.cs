using Memory;

namespace HobbitSpeedrunTools
{
    public class DevMode : ToggleCheat
    {
        public override string Name { get; set; } = "Developer Mode";
        public override string ShortName { get; set; } = "DEV";
        public override string ShortcutName { get; set; } = "dev_mode";
        public override string ToolTip { get; set; } = "Enables Developer Mode. The developer menu can be accessed by pressing tidle (~) in game.\n" +
            "While active, fly mode can be enabled with Ctrl + F.";

        public DevMode(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.devMode, "int", Enabled ? "1" : "0");
        }
    }
}
