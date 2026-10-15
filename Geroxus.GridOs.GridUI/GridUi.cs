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
            ((TextNode)Ui.RootNode.Children[0]).AppendLine(LOGGER.GetLog(30));
            // first get all currently available Displays
            // then write to displays as necessary
            _builder.AppendLine($"Welcome to GridOS v{GridOs.Instance.VersionString}");
            _builder.AppendLine($"Online for {GridOs.Instance.ActiveTime}");
            _builder.Append(Environment.NewLine);
            _write(_builder.ToString());
            _builder.Clear();

            foreach (GridUiHandler uiComponent in OsUiBridge.Instance.AllComponents.Values)
            {
                LOGGER.Info($"{uiComponent.Program.ToString()}");
                foreach (DisplayDriver display in _displaysByProgram.GetValueOrDefault(uiComponent.Program, new List<DisplayDriver>()))
                {
                    //traverse Tree (NOT recursive to avoid possible memory limits)
                    //TODO replace with while(length(AllChildren)>0) to scan
                    Queue<IGridUiNode> scanQueue = new Queue<IGridUiNode>();
                    scanQueue.Enqueue(uiComponent.RootNode);
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

        public override List<string> GetRequirements()
        {
            return new List<string>();
        }

        public override void SetUp()
        {
            Ui.Program = GridProgram.GridUi;
            Ui.RootNode
                .AddDrawCondition(new GridUiDrawConditionAlways())
                .CreateChildNode<TextNode>()
                .AddDrawCondition(new GridUiDrawConditionAlways());
            
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