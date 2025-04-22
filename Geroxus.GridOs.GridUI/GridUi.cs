using System;
using System.Linq;

namespace IngameScript
{
    public class GridUi : IGridUi
    {
        private readonly OsProcessBridge _processBridge = OsProcessBridge.Instance;

        public GridUi(ProcessId processId, string name)
        {
            Name = name;
            ProcessId = processId;
        }

        public string Name { get; }

        public ProcessId ProcessId { get; }

        public void Run()
        {
            Action<string> write = text => _processBridge.GetDrivers(typeof(DisplayDriver)).ForEach(d => (d as DisplayDriver)?.AppendLine(text));
            
            foreach (string info in _processBridge.GetServices().Select(s => s.Info)) write(info);

            write(Environment.NewLine);
            write("Running Processes:");
            _processBridge.GetAllProcesses().ForEach(p => write($"{p.ProcessId.Id, 6}: {p.Name}"));
        }

        public void Dispose()
        {
        }
    }
}