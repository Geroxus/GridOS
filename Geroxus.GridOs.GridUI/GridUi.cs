using System;
using System.Linq;

namespace IngameScript
{
    public class GridUi : IGridUi
    {
        private readonly OsProcessBridge _processBridge = OsProcessBridge.Instance;
        private Action<string> _write;

        public GridUi(ProcessId processId, string name)
        {
            Name = name;
            ProcessId = processId;
        }

        public string Name { get; }

        public ProcessId ProcessId { get; }

        public void Run()
        {
            foreach (string info in _processBridge.GetServices().Select(s => s.Info)) _write(info);

            _write(Environment.NewLine);
            _write("Running Processes:");
            _processBridge.GetAllProcesses()
                .Where(p => p.ProcessId.Id < 90000).ToList()
                .ForEach(p => _write($"{p.ProcessId.Id, 6}: {p.Name}"));
        }

        public void SetUp()
        {
            _write = text => _processBridge.GetDrivers(typeof(DisplayDriver))
                .Where(d => (d as DisplayDriver)?.Program == GridProgram.NONE)
                .ToList()
                .ForEach(d => (d as DisplayDriver)?.AppendLine(text));
        }

        public static void Register()
        {
           ProgramFactory.Register(new GridUiFactory()); 
        }

        public void Dispose()
        {
        }
    }
}