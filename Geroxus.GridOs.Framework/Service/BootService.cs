using System;
using System.Linq;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class BootService : IGridService
    {
        public string Name { get; }
        public ProcessId ProcessId { get; }
        private OsProcessBridge Processes { get; } = OsProcessBridge.Instance;
        private OsGridAccessBridge Grid { get; } = OsGridAccessBridge.Instance;

        public BootService(ProcessId processId)
        {
            Name = "GridOS Boot Service";
            ProcessId = processId;
        }

        public void Run()
        {
            Grid.Get<IMyTextSurface>(DriverFactory.Get).ForEach(Processes.Register);
            LOGGER.Write("Boot registered displays");

            Processes.Register(ProgramFactory.Get<GridUI>());
            Processes.Register(ProgramFactory.Get<FlightCapability>());

            // displaying stuff
            LOGGER.Write("Boot finished, Stop Booting");
            Processes.RegisterStop(ProcessId);
        }

        public void Dispose()
        {
        }
    }
}