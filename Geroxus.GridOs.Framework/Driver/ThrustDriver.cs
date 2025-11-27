using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public class ThrustDriver : IGridDriver
    {
        private readonly IMyThrust _thrust;
        private readonly Vector3I _assumedDirection;
        private readonly Vector3I? _setDirection = null;

        public ThrustDriver(IMyThrust thrust, ProcessId processId, string name, Vector3I setDirection)
        {
            Name = name;
            ProcessId = processId;
            
            _thrust = thrust;
            // last known direction of this thruster. Needs an active cockpit to be set
            _assumedDirection = _thrust.GridThrustDirection;
            if (setDirection != Vector3I.Zero)
                _setDirection = setDirection;
        }
        
        public Vector3I Direction => _setDirection ?? _assumedDirection;

        public float MaxThrust => _thrust.MaxThrust;
        public float CurrentThrust => _thrust.CurrentThrust;
        
        public string Name { get; }
        public ProcessId ProcessId { get; }
        public void Dispose()
        {
        }
        public void Run()
        {
        }

        public void SetUp()
        {
        }
    }
}