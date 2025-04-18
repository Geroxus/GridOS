using System;

namespace IngameScript
{
    public class GridUI : IGridProgram
    {
        private readonly OsProcessBridge _processBridge = OsProcessBridge.Instance;

        public GridUI(ProcessId processId, string name)
        {
            Name = name;
            ProcessId = processId;
        }

        public string Name { get; }

        public ProcessId ProcessId { get; }

        public void Run()
        {
            Action<string> write = text => _processBridge.GetDrivers(typeof(DisplayDriver)).ForEach(d => (d as DisplayDriver)?.AppendLine(text));
            LOGGER.Write("Write displays");
            write("Running Processes:");

            _processBridge.GetAllProcesses().ForEach(p => write($"{p.ProcessId.Id, 6}: {p.Name}"));
        }

        public void Dispose()
        {
        }
    }
}