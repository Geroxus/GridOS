namespace IngameScript
{
    public interface IGridOsProcess : IDisposable
    {
        string Name { get; }
        int ProcessId { get; }
    }
}