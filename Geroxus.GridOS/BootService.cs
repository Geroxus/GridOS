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

        public BootService(ProcessId processId, String name)
        {
            Name = name;
            ProcessId = processId;
        }

        public void Run()
        {
            Grid.Get<IMyTextSurface>(DriverFactory.Get).ForEach(Processes.Register);
            Grid.Get<IMyShipController>(DriverFactory.Get).ForEach(Processes.Register);
            LOGGER.Write("Driver registration complete");

            Processes.Register(ProgramFactory.Get<GridUi>());
            Processes.Register(ProgramFactory.Get<FlightCapability>());

            // displaying stuff
            LOGGER.Write("Boot finished, Stop Booting");
            Processes.RegisterStop(ProcessId);
        }

        public void Dispose()
        {
        }

        public string Info { get; } = "BootService available...";
    }
}