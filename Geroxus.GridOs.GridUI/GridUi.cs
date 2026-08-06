using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IngameScript
{
    public class GridUi : GridProgramBase
    {
        private readonly OsProcessBridge _processBridge = OsProcessBridge.Instance;
        private Action<string> _write;
        private StringBuilder _builder = new StringBuilder();
        private readonly Dictionary<GridProgram, List<DisplayDriver>> _displaysByProgram =  new Dictionary<GridProgram, List<DisplayDriver>>();

        public override void Run()
        {
            // first get all currently available Displays
            // then write to displays as necessary
            _builder.AppendLine($"Welcome to GridOS v{GridOs.Instance.VersionString}");
            _builder.AppendLine($"Online for {GridOs.Instance.ActiveTime}");
            _builder.Append(Environment.NewLine);
            _write(_builder.ToString());
            _builder.Clear();

            foreach (GridUiHandler uiComponent in OsUiBridge.Instance.AllComponents.Values)
            {
                foreach (DisplayDriver display in _displaysByProgram[uiComponent.Program])
                {
                    //traverse Tree (NOT recursive to avoid possible memory limits)
                    //TODO replace with while(length(AllChildren)>0) to scan
                    Queue<IGridUiNode> scanQueue = new Queue<IGridUiNode>(uiComponent.RootNode.Children);
                    while (scanQueue.Count > 0)
                    {
                        IGridUiNode child = scanQueue.Dequeue();
                        if (child.Conditions.All(c => c.Evaluate(display.Settings)))
                        {
                            if(child.GetType() == typeof(TextNode))
                                _builder.Append(((TextNode)child).Out());
                            else if (child.GetType() == typeof(ContainerNode))
                                child.Children.ForEach(scanQueue.Enqueue);
                        }
                    }
                    display.DisplayText.Append(_builder);
                    _builder.Clear();
                }
            }

            _builder.Clear();
        }

        public override void SetUp()
        {
            ScanGridForDisplays();
            
            // assign output to this horrible piece of code
            _write = text => _processBridge.GetDrivers(typeof(DisplayDriver))
                .Where(d => (d as DisplayDriver)?.Program == GridProgram.NONE)
                .ToList()
                .ForEach(d => (d as DisplayDriver)?.AppendLine(text));
        }

        private void ScanGridForDisplays()
        {
            var groupedDrivers = OsProcessBridge.Instance.GetDrivers(typeof(DisplayDriver))
                .OfType<DisplayDriver>()
                .GroupBy(d => d.Program);
            foreach (var group in groupedDrivers)
            {
                _displaysByProgram.GetValueOrNew(group.Key).AddList(group.ToList());
            }
        }
    }
}