using System.Collections;
using System.Collections.Generic;

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

        public override bool Equals(object obj)
        {
            if (!(obj is ProcessId)) return false;
            return Id.Equals((obj as ProcessId).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}