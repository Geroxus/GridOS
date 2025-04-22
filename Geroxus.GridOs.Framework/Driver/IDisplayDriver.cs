namespace IngameScript
{
    public interface IDisplayDriver: IGridDriver
    {
        void AppendLine(string line);
    }
}