namespace IngameScript
{
    internal class ProcessIdProvider
    {
        private ProcessId ProcessIdBasis { get; }
        private ushort CurrentOffset { get; set; }

        public ProcessIdProvider(ProcessId initialProcessId)
        {
            ProcessIdBasis = initialProcessId;
            CurrentOffset = 0;
        }

        public ProcessId Next()
        {
            CurrentOffset++;
            return ProcessIdBasis.FromOffset(CurrentOffset);
        }
        
    }
}