using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public class EnrichedThrust : IEnrichedComponent<IMyThrust>
    {
        private readonly string _direction;
        public Vector3I definedDirection;

        public EnrichedThrust(IMyThrust component, string direction)
        {
            Component = component;
            Name = component.DisplayNameText;
            _direction = direction;
        }
        
        public Vector3I GetDirection() {
            switch (_direction)
            {
               case "Forward": return Vector3I.Forward;
               case "Backward": return Vector3I.Backward;
               case "Left": return Vector3I.Left;
               case "Right": return Vector3I.Right;
               case "Up": return Vector3I.Up;
               case "Down": return Vector3I.Down;
               case "none": return Vector3I.Zero;
               default: return Vector3I.Zero;
            }
        }

        public IMyThrust Component { get; }
        public string Name { get; }
    }
}