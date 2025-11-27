using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public class FlightCapability : IGridService<FlightCapabilityInfo>
    {
        public FlightCapabilityInfo Info { get; private set; }
        
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
        private float _forceOnShip = 0;
        private GridOsPlanet _selectedPlanet = new GridOsPlanet("Mars", new PhysicsValue<float>(PhysicsUnit.NewtonPerKilogram, 9.80665F * 0.9F));

        public void Run()
        {
            _builder.AppendLine("Flight Capability Observer:");
            FlightCapabilityInfo info = new FlightCapabilityInfo();
            InputDriver[] inputDrivers = OsProcessBridge.Instance.GetDrivers(typeof(InputDriver)).OfType<InputDriver>().ToArray();
            List<InputDriver> controlledInput = inputDrivers.Where(d => d.IsControlled).ToList();
            if (inputDrivers.Any(d => d.Component.GetNaturalGravity().Equals(Vector3.Zero)))
                _builder.AppendLine("Currently in space");
            else
            {
                // TODO this produces weird outputs when two controllers are present. Fix it
                foreach (InputDriver inputDriver in controlledInput)
                {
                    Vector3D naturalGravity = inputDriver.Component.GetNaturalGravity();
                    MyShipMass shipMass = inputDriver.Component.CalculateShipMass();
                    _builder.AppendLine($"Down: {_maxThrustPerDirection[Vector3I.Up]/1000}kN, Backward: {_maxThrustPerDirection[Vector3I.Forward]/1000}kN, Left: {_maxThrustPerDirection[Vector3I.Right]/1000}kN");
                    _builder.AppendLine($"Up: {_maxThrustPerDirection[Vector3I.Down]/1000}kN, Forward: {_maxThrustPerDirection[Vector3I.Backward]/1000}kN, Right: {_maxThrustPerDirection[Vector3I.Left]/1000}kN");
                    _forceOnShip = (float)((double)shipMass.PhysicalMass * naturalGravity.Length());
                    _builder.AppendLine($"Safe to land on {_selectedPlanet.Name}({_selectedPlanet.Gravity.Value :F2}N/kg)? {(CanSustainFlight(shipMass, _selectedPlanet) ? "yes" : "no")}");
                    
                    info.ShipMass = new PhysicsValue<float>(PhysicsUnit.Kilogram, shipMass.PhysicalMass);
                    info.ShipGravity = new PhysicsValue<float>(PhysicsUnit.Newton, _forceOnShip);
                    info.NaturalGravity = new PhysicsValue<float>(PhysicsUnit.NewtonPerKilogram, (float)naturalGravity.Length());
                    info.CurrentFlightSustain = new FlightSustain(CanSustainFlight());
                    info.TargetFlightSustain = new FlightSustain(CanSustainFlight(shipMass, _selectedPlanet), _selectedPlanet);
                }
            }

            Info = info;
            _builder.Clear();
        }

        private bool CanSustainFlight(MyShipMass shipMass, GridOsPlanet planet)
        {
            return shipMass.PhysicalMass * planet.Gravity.Value < _maxThrustPerDirection.Values.Max();
        }

        private bool CanSustainFlight()
        {
            return _forceOnShip < _maxThrustPerDirection.Values.Max();
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

    public struct FlightSustain
    {
        public FlightSustain(bool canSustainFlight, GridOsPlanet? target = null)
        {
            CanSustainFlight =  canSustainFlight;
            Target = target;
        }

        /**
         * Can be null. Indicates that this is calculated with regard to currently applied natural gravity
         */
        public GridOsPlanet? Target { get; set; }

        public bool CanSustainFlight { get; set; }
    }

    public struct GridOsPlanet
    {
        public readonly String Name;
        public readonly PhysicsValue<float> Gravity;

        public GridOsPlanet(string name, PhysicsValue<float> gravity)
        {
            Name = name;
            Gravity = gravity;
        }
    }

    public struct FlightCapabilityInfo
    {
        public PhysicsValue<float> ShipMass { get; set; }
        public PhysicsValue<float> ShipGravity { get; set; }
        public PhysicsValue<float> NaturalGravity { get; set; }
        public Dictionary<Vector3I, PhysicsValue<float>> MaxThrustPerDirection { get; set; }
        public Dictionary<Vector3I, PhysicsValue<float>> ThrustPerDirection { get; set; }
        public FlightSustain CurrentFlightSustain { get; set; }
        public FlightSustain TargetFlightSustain { get; set; }
    }
}