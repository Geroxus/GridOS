using System;

namespace IngameScript
{
    public class ProcessIdProvider
    {
        private ProcessId ProgramProcessIdBasis { get; } = new ProcessId(1000);
        private ushort CurrentProgramProcessIdOffset { get; set; } = 0;
        private ProcessId ServiceProcessIdBasis { get; } = new ProcessId(40000);
        private ushort CurrentServiceProcessIdOffset { get; set; } = 0;
        private ProcessId DriverProcessIdBasis { get; } = new ProcessId(90000);
        private ushort CurrentDriverProcessIdOffset { get; set; } = 0;


        public ProcessId Next(Type type)
        {
            if (type == typeof(IGridProgram))
            {
                CurrentProgramProcessIdOffset++;
                return ProgramProcessIdBasis.FromOffset(CurrentProgramProcessIdOffset);
            } else if (type == typeof(IGridService))
            {
                CurrentServiceProcessIdOffset++;
                return ServiceProcessIdBasis.FromOffset(CurrentServiceProcessIdOffset);
            } else if (type == typeof(IGridDriver))
            {
                CurrentDriverProcessIdOffset++;
                return DriverProcessIdBasis.FromOffset(CurrentDriverProcessIdOffset);
            }
            throw new Exception($"Unsupported type {type.Name}");
        }
        
    }
}