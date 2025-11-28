namespace IngameScript
{
    public class DoorDriver : IGridDriver
    {
        public DoorDriver(ProcessId processId, string name)
        {
            ProcessId = processId;
            Name = name;
        }

        public void Dispose()
        {
        }

        public string Name { get; }
        public ProcessId ProcessId { get; }
        public void Run()
        {
        }

        public void SetUp()
        {
        }
    }
}