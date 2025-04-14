namespace IngameScript
{
    internal class ProcessIdProvider
    {
        private readonly int _processIdBasis;
        private ushort _currentOffset;

        public ProcessIdProvider(int initialProcessId)
        {
            _processIdBasis = initialProcessId;
            _currentOffset = 0;
        }

        public int Next()
        {
            _currentOffset++;
            return _processIdBasis + _currentOffset;
        }
        
    }
}