using System.Text;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class DisplayDriver : IGridDriver
    {
        public string Name { get; }
        public ProcessId ProcessId { get; }
        
        
        private readonly IMyTextSurface _component;
        
        private StringBuilder DisplayText { get; } = new StringBuilder();

        public DisplayDriver(IMyTextSurface component, ProcessId processId, string name)
        {
            _component = component;
            
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