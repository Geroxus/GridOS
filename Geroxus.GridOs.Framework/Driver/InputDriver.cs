using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class InputDriver : IGridDriver
    {
        public InputDriver(IMyShipController shipController, ProcessId processId, string name)
        {
            ProcessId = processId;
            Name = name;
            
            Component = shipController;
        }

        /// <deprecated/>
        /// Should not be used under any circumstances and is scheduled to be made private.
        public IMyShipController Component { get; set; }

        public void Dispose()
        {
        }

        public string Name { get; }
        public ProcessId ProcessId { get; }
        public void Run()
        {
            
        }
    }
}