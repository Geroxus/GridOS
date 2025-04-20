using System;

namespace IngameScript
{
    public interface IProcessFactory<out T>
    {
        Func<string, T> CreateProcess(ProcessIdProvider idProvider);
    }
}