using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public class ThrustDriver : GridDriverBase<EnrichedThrust>
    {
        private IMyThrust _thrust;
        private Vector3I _assumedDirection;
        private Vector3I? _setDirection = null;
        
        public Vector3I Direction => _setDirection ?? _assumedDirection;

        public float MaxThrust => _thrust.MaxThrust;
        public float CurrentThrust => _thrust.CurrentThrust;
        
        public override void Run()
        {
        }

        public override void SetUp()
        {
            _thrust = Component.Component;
            // last known direction of this thruster. Needs an active cockpit to be set
            _assumedDirection = Component.Component.GridThrustDirection;
            if (Component.GetDirection() != Vector3I.Zero)
                _setDirection = Component.GetDirection();
        }
    }
}