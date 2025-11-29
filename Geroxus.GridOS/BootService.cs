using System;
using System.Linq;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class BootService : GridServiceBase
    {
        private OsProcessBridge Processes { get; } = OsProcessBridge.Instance;
        private OsGridAccessBridge Grid { get; } = OsGridAccessBridge.Instance;

        public override void Run()
        {
        }

        public override void SetUp()
        {
            Grid.Get<IMyTextSurface>(DriverFactory.Get<EnrichedTextSurface>()).ForEach(Processes.Register);
            Grid.Get<IMyShipController>(DriverFactory.Get<EnrichedShipController>()).ForEach(Processes.Register);
            Grid.Get<IMyThrust>(DriverFactory.Get<EnrichedThrust>()).ForEach(Processes.Register);
            LOGGER.Info("Driver registration complete");

            Processes.Register(ProgramFactory.Create<FlightCapability>());
            
            Processes.Register(ProgramFactory.Create<LiveStats>());
            Processes.Register(ProgramFactory.Create<GridUi>());

            // displaying stuff
            LOGGER.Info("Boot finished, Stop Booting");
            Processes.RegisterStop(ProcessId);
        }

    }
}