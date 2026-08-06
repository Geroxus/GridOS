namespace IngameScript
{
    public abstract class GridServiceBase : IGridService
    {
        public  string Name { get; private set; }
        public  ProcessId ProcessId { get; private set; }
        public void Initialize(ProcessIdProvider processIdProvider, string name)
        {
            ProcessId = processIdProvider.Next(typeof(IGridService));
            Name = name;
        }
        public void Dispose()
        {
        }
        public abstract void Run();
        public abstract void SetUp();
    }

    public abstract class GridServiceBase<T> : GridServiceBase, IGridService<T>
    {
        public abstract T Info { get; }
    }
}