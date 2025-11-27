using System;

namespace IngameScript
{
    public interface IGridOsProcess : IDisposable
    {
        string Name { get; }
        ProcessId ProcessId { get; }
        /**
         * This Method is the main execution loop which will be called each time the OS updates
         */
        void Run();
        /**
         * This Method is called exactly once during its setup. No guarantees regarding specific running processes can be made
         */
        void SetUp();
    }
}