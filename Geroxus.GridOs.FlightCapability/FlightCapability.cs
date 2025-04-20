namespace IngameScript
{
    public class FlightCapability : IGridProgram
    {
        public FlightCapability(ProcessId processId, string name)
        {
            Name = name;
            ProcessId = processId;
        }

        public static void Register()
        {
            ProgramFactory.Register(typeof(FlightCapability).ToString(), new FlightCapabilityFactory());
        }

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