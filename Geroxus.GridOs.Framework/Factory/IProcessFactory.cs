using System;

namespace IngameScript
{
    public interface IProcessFactory<out T>
    {
        Func<string, T> CreationStrategy(ProcessIdProvider idProvider);

        string ProcessName { get; }
    }
}