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
            // .RegisterDriver(DriverFactory.Get<IMyTextSurface>(Me.GetSurface(0)))
            Grid.Get<IMyTextSurface>(DriverFactory.Get).ForEach(Processes.RegisterDriver);
            LOGGER.Write("Boot registered displays");

            
            // displaying stuff
            Action<string> write = text => Processes.GetDrivers(typeof(DisplayDriver)).ForEach(d => (d as DisplayDriver)?.AppendLine(text));
            write("Booting...");
            
            Processes.GetAllProcesses().ForEach(p => write($"{p.ProcessId.Id, 6}: {p.Name}"));
        }

        public void Dispose()
        {
        }
    }
}