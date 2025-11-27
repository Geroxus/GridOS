using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public class FlightCapability : IGridService
    {
        public string Info { get; private set; }
        
        public FlightCapability(ProcessId processId, string name)
        {
            Name = name;
            ProcessId = processId;
        }

        public static void Register()
        {
            ProgramFactory.Register(new FlightCapabilityFactory());
        }

        public void Dispose()
        {
        }

        public string Name { get; }
        public ProcessId ProcessId { get; }

        private readonly StringBuilder _builder = new StringBuilder();
        private Dictionary<Vector3I, float> _maxThrustPerDirection;

        public void Run()
        {
            _builder.AppendLine("Flight Capability Observer:");
            InputDriver[] inputDrivers = OsProcessBridge.Instance.GetDrivers(typeof(InputDriver)).OfType<InputDriver>().ToArray();
            List<InputDriver> controlledInput = inputDrivers.Where(d => d.IsControlled).ToList();
            if (inputDrivers.Any(d => d.Component.GetNaturalGravity().Equals(Vector3.Zero)))
                _builder.AppendLine("Currently in space");
            else
            {
                foreach (InputDriver inputDriver in controlledInput)
                {
                    Vector3D naturalGravity = inputDriver.Component.GetNaturalGravity();
                    MyShipMass shipMass = inputDriver.Component.CalculateShipMass();
                    _builder.AppendLine($"{inputDriver.Name}:");
                    _builder.AppendLine($"Gravity: {naturalGravity.Length()}");
                    _builder.AppendLine($"Mass: {shipMass.TotalMass}kg({shipMass.BaseMass}kg)");
                    _builder.AppendLine($"Down: {_maxThrustPerDirection[Vector3I.Up]/1000}, Backward: {_maxThrustPerDirection[Vector3I.Forward]/1000}, Left: {_maxThrustPerDirection[Vector3I.Right]/1000}");
                    _builder.AppendLine($"Up: {_maxThrustPerDirection[Vector3I.Down]/1000}, Forward: {_maxThrustPerDirection[Vector3I.Backward]/1000}, Right: {_maxThrustPerDirection[Vector3I.Left]/1000}");
                }
            }
            
            // get max force per direction
            
            Info = _builder.ToString();
            _builder.Clear();
        }

        public void SetUp()
        {
            CalculateMaxThrustPerDirection();
        }

        private void CalculateMaxThrustPerDirection()
        {
            _maxThrustPerDirection = SumPerDirection(d => d.MaxThrust);
        }

        private Dictionary<Vector3I, float> SumPerDirection(Func<ThrustDriver, float> thrustAccessFunction)
        {
            List<ThrustDriver> thrustDrivers = OsProcessBridge.Instance.GetDrivers(typeof(ThrustDriver)).OfType<ThrustDriver>().ToList();
            Dictionary<Vector3I, float> thrustPerDirection = new Dictionary<Vector3I, float>();
            foreach (ThrustDriver thrustDriver in thrustDrivers)
            {
                float thrust = thrustPerDirection.GetValueOrDefault(thrustDriver.Direction, 0);
                thrust += thrustAccessFunction(thrustDriver);
                thrustPerDirection[thrustDriver.Direction] = thrust;
            }
            return thrustPerDirection;
        }
    }
}