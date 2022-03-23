using Memory;

namespace HobbitSpeedrunTools.cheats
{
    public class DevMode : ToggleCheat
    {
        public new readonly string shortName = "DEV";
        public new readonly string shortcutName = "dev_mode";

        public DevMode(Mem _mem)
        {
            mem = _mem;
        }

        public override void OnTick()
        {
            mem?.WriteMemory(MemoryAddresses.devMode, "int", enabled ? "1" : "0");
        }
    }
}
