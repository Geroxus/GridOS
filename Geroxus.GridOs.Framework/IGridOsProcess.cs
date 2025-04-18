using System;

namespace IngameScript
{
    public interface IGridOsProcess : IDisposable
    {
        string Name { get; }
        ProcessId ProcessId { get; }
        void Run();
    }
}