using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace IngameScript
{
    public class LiveStats : IGridProgram
    {
        private readonly List<DisplayDriver> _displays;

        public LiveStats(ProcessId processId, string name)
        {
            ProcessId = processId;
            Name = name;
            
            _displays = new List<DisplayDriver>();
        }

        public void Dispose()
        {
        }

        public string Name { get; }
        public ProcessId ProcessId { get; }

        public void Run()
        {
            // generate text to output

            // write output to displays
            LOGGER.Always($"Live Stats running with {_displays.Count} displays");
            foreach (DisplayDriver display in _displays)
            {
                display.AppendLine("Welcome to LiveStats! Here you'll soon find the stats. Live! Wow!");
            }
        }

        public void SetUp()
        {
            //look through all displays and select those that should display stats
            // subsection GridOS.Program key program=LiveStats key setting=ProcessList
            ImmutableList<IGridDriver> drivers = OsProcessBridge.Instance.GetDrivers(typeof(DisplayDriver));
            foreach (IGridDriver driver in drivers)
            {
                DisplayDriver display = driver as DisplayDriver;
                if (display == null) throw new Exception("The display driver should never be null! CRITICAL");

                switch (display.Program)
                {
                    case GridProgram.NONE:
                        continue;
                    case GridProgram.LiveStats:
                        _displays.Add(display);
                        break;
                }
            }
        }

        public static void Register()
        {
            ProgramFactory.Register(new LiveStatsFactory());
        }
    }
}