using System;
using System.Collections.Immutable;

namespace IngameScript
{
    public class LiveStats : IGridProgram
    {
        public LiveStats(ProcessId processId, string name)
        {
            ProcessId = processId;
            Name = name;
        }

        public void Dispose()
        {
            
        }

        public string Name { get; }
        public ProcessId ProcessId { get; }
        public void Run()
        {
            
        }

        public void SetUp()
        {
            //look through all displays and select those that should display stats
            // subsection GridOS.Program key program=LiveStats key setting=ProcessList
            ImmutableList<IGridDriver> drivers = OsProcessBridge.Instance.GetDrivers(typeof(IDisplayDriver));
            foreach (IGridDriver driver in drivers)
            {
                DisplayDriver display = driver as DisplayDriver;
                if (display == null) throw new Exception("The display driver should never be null! CRITICAL");

                switch (display.Program)
                {
                    
                }
            }
        }
    }
}