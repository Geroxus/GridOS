namespace IngameScript
{
    public interface IEnrichedComponent<T>
    {
        T Component { get; }
        string Name { get; }
    }
}