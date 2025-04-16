namespace IngameScript
{
    public abstract class GridService
    {
        protected readonly OsProcessBridge Processes = OsProcessBridge.Instance;

        public abstract void Run();
    }
}