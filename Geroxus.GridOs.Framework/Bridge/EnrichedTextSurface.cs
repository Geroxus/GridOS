using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class EnrichedTextSurface : IEnrichedComponent<IMyTextSurface>
    {
        public EnrichedTextSurface(IMyTextSurface myTextSurface, string name, GridProgram program)
        {
            Component = myTextSurface;
            Name = name;
            Program = program;
        }

        public IMyTextSurface Component { get; }
        public string Name { get; }
        public GridProgram Program { get; }
    }
}