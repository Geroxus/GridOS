namespace IngameScript
{
    public class ProcessId
    {
        public ProcessId(uint id)
        {
            Id = id;
        }

        public uint Id { get; }

        public ProcessId FromOffset(ushort offset)
        {
            return new ProcessId((uint)(Id + offset));
        }
    }
}