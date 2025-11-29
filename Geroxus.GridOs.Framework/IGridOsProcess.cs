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
         * This Method is called during its setup. It should never be called before boot initializations are done.
         * It can be called again to reset the class to an initial state of sorts.
         */
        void SetUp();
        /**
         * This Method should be called exactly once after it has been initalized and never manually by any
         * method other than the factory.
         */
        void Initialize(ProcessIdProvider processIdProvider, string name);
    }
}