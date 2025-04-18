using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    internal class EnrichedTextSurface : IEnrichedComponent<IMyTextSurface>
    {
        public EnrichedTextSurface(IMyTextSurface myTextSurface, string name)
        {
            Component = myTextSurface;
            Name = name;
        }

        public IMyTextSurface Component { get; }
        public string Name { get; }
    }
}