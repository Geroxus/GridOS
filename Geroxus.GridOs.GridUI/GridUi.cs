using System;
using System.Linq;

namespace IngameScript
{
    public class GridUi : GridProgramBase
    {
        private readonly OsProcessBridge _processBridge = OsProcessBridge.Instance;
        private Action<string> _write;

        public override void Run()
        {
            _write($"Welcome to GridOS v{GridOs.Version}");
            _write(Environment.NewLine);

        }

        public override void SetUp()
        {
            _write = text => _processBridge.GetDrivers(typeof(DisplayDriver))
                .Where(d => (d as DisplayDriver)?.Program == GridProgram.NONE)
                .ToList()
                .ForEach(d => (d as DisplayDriver)?.AppendLine(text));
        }
    }
}