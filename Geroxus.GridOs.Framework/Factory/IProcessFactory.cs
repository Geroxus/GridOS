using System;

namespace IngameScript
{
    public interface IProcessFactory<out T>
    {
        Func<ProcessId, string, T> CreateProcess();
    }
}