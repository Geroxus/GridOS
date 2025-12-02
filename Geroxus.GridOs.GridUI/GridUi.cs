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

        public override void Run()
        {
            _builder.AppendLine($"Welcome to GridOS v{GridOs.Instance.VersionString}");
            _builder.AppendLine($"Online for {GridOs.Instance.ActiveTime}");
            _builder.Append(Environment.NewLine);

            foreach (GridUiHandler uiComponent in OsUiBridge.Instance.AllComponents.Values)
            {
                foreach (IGridUiNode child in uiComponent.RootNode.Children)
                {
                    if (child.GetType() == typeof(TextNode))
                    {
                        _builder.Append(((TextNode)child).Out());
                    }
                }
            }
            
            _write(_builder.ToString());
            _builder.Clear();
        }

        public override void SetUp()
        {
            _write = text => _processBridge.GetDrivers(typeof(DisplayDriver))
                .Where(d => (d as DisplayDriver)?.Program == GridProgram.NONE)
                .ToList()
                .ForEach(d => (d as DisplayDriver)?.AppendLine(text));
        }


        public void Display(String text)
        {
            _builder.AppendLine(text);
        }
    }
}