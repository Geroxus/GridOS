using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class EnrichedTextSurface : IEnrichedComponent<IMyTextSurface>
    {

        public EnrichedTextSurface(IMyTextSurface myTextSurface, string name, GridProgram program, string settings, int surfaceIdentifier, long parentId)
        {
            Component = myTextSurface;
            Name = name;
            Program = program;
            Settings = settings;
            SurfaceIdentifier = surfaceIdentifier;
            ParentId = parentId;
        }

        public IMyTextSurface Component { get; }
        public string Name { get; }
        public GridProgram Program { get; }
        public string Settings { get; }
        public int SurfaceIdentifier { get; set; }
        public long ParentId { get; }
    }
}