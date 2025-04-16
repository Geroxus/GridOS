using System;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class BootService : GridService, IGridOsProcess
    {
        public string Name { get; }
        public ProcessId ProcessId { get; }

        public BootService(ProcessId processId)
        {
            Name = "GridOS Boot Service";
            ProcessId = processId;
        }

        public override void Run()
        {
            Action<string> write = text => Processes.GetDrivers(typeof(DisplayDriver)).ForEach(d => (d as DisplayDriver)?.AppendLine(text));
            write("Booting...");
            
            Processes.GetAllProcesses().ForEach(p => write($"{p.ProcessId.Id, 6}: {p.Name}"));
        }

        public void Dispose()
        {
        }
    }
}