namespace IngameScript
{
    public interface IEnrichedComponent
    {
        string Name { get; }
    }
    public interface IEnrichedComponent<T> : IEnrichedComponent
    {
        T Component { get; }
    }
}