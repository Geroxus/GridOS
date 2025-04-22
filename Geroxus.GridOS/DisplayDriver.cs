using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;

namespace IngameScript
{
    public class DisplayDriver : IDisplayDriver
    {
        public string Name { get; }
        public ProcessId ProcessId { get; }
        
        
        private readonly IMyTextSurface _component;
        
        private StringBuilder DisplayText { get; } = new StringBuilder();

        public DisplayDriver(IMyTextSurface component, ProcessId processId, string name)
        {
            _component = component;

            _component.ContentType = ContentType.TEXT_AND_IMAGE;
            _component.WriteText("");
            
            Name = name;
            ProcessId = processId;
        }

        public void Run()
        {
            _component.WriteText(DisplayText);
            DisplayText.Clear();
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }

        public void AppendLine(string text)
        {
            DisplayText.AppendLine(text);
        }
    }
}