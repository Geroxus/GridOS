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
        public void Run()
        {
            _builder.AppendLine("FlightCapability running...");
            InputDriver[] inputDrivers = OsProcessBridge.Instance.GetDrivers(typeof(InputDriver)).OfType<InputDriver>().ToArray();
            if (inputDrivers.Any(d => d.Component.GetNaturalGravity().Equals(Vector3.Zero)))
                _builder.AppendLine("Currently in space");
            else
            {
                _builder.AppendLine("NOT IMPLEMENTED");
                foreach (InputDriver inputDriver in inputDrivers)
                {
                    Vector3D naturalGravity = inputDriver.Component.GetNaturalGravity();
                    MyShipMass shipMass = inputDriver.Component.CalculateShipMass();
                    _builder.AppendLine($"{inputDriver.Name}:");
                    _builder.AppendLine($"Gravity: {naturalGravity.Length()}");
                    _builder.AppendLine($"Mass: {shipMass.TotalMass}kg({shipMass.BaseMass}kg)");
                }
            }
            
            Info = _builder.ToString();
            _builder.Clear();
        }
    }
}