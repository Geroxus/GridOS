using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class DisplayDriver : IGridDriver
    {
        public string Name { get; }
        public int ProcessId { get; }
        
        
        private readonly IMyTextSurface _component;

        public DisplayDriver(IMyTextSurface component, int processId)
        {
            _component = component;
            
            Name = $"DisplayDriver: {_component.Name}";
            ProcessId = processId;
        }

        public void Update()
        {
            
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}