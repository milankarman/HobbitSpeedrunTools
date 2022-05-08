using Memory;

namespace HobbitSpeedrunTools
{
    public class QuickLoad : ActionCheat
    {
        public override string Name { get; set; } = "Quick Load";
        public override string ShortcutName { get; set; } = "quick_load";
        public TimerManager? TimerManager { get; set; } = null;

        public QuickLoad(Mem _mem, TimerManager _timerManager)
        {
            mem = _mem;
            TimerManager = _timerManager;
        }

        public override void Start()
        {
            mem?.WriteMemory(MemoryAddresses.stamina, "float", "10");
            mem?.WriteMemory(MemoryAddresses.bilboState, "int", "27");

            if (TimerManager != null) TimerManager.stopped = false;
        }
    }
}
