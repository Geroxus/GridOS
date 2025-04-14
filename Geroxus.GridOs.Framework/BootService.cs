using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class BootService : GridService, IGridOsProcess
    {
        public string Name { get; }
        public int ProcessId { get; }

        public BootService(int processId)
        {
            Name = "GridOS Boot Service";
            ProcessId = processId;
        }

        public override void Run()
        {
            Os.GetDrivers(typeof(DisplayDriver)).ForEach(d => (d as DisplayDriver).AddLine("Boot is Up"));
        }

        public void Dispose()
        {
        }
    }
}