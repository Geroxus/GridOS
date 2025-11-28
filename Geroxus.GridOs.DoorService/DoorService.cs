namespace IngameScript
{
    public class DoorService : IGridService
    {
        public DoorService(ProcessId processId, string name)
        {
            ProcessId = processId;
            Name = name;
        }

        public static void Register()
        {
            ProgramFactory.Register(new DoorDriverFactory());
            ProgramFactory.Register(new DoorServiceFactory());
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