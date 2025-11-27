namespace IngameScript
{
    public interface IGridService : IGridOsProcess
    {
    }
    public interface IGridService<out TInfo> : IGridService
    {
        TInfo Info { get; }
    }
}