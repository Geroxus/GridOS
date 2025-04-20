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
    public class BootServiceFactory : IProcessFactory<BootService>
    {
        public Func<string, BootService> CreateProcess(ProcessIdProvider idProvider) =>
            (name) => new BootService(idProvider.Next(typeof(IGridService)), "GridOS Boot Service");
    }
}