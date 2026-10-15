using System;
using System.Collections.Generic;

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
         * Lists the expected names of the processes needed for function. Without them function cannot be guaranteed.
         */
        List<String> GetRequirements();
        /**
         * This Method is called during its setup. It should never be called before boot initializations are done.
         */
        void SetUp();
        /**
         * This Method should be called exactly once after it has been initalized and never manually by any
         * method other than the factory.
         */
        void Initialize(ProcessIdProvider processIdProvider, string name);
    }
}