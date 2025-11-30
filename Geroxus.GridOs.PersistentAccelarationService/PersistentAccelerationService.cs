using System;
using System.Linq;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public class PersistentAccelerationService : GridServiceBase<AccelarationInformation>
    {
        private GridUiHandler _ui;
        private int? _previousValue = null;

        public override void Run()
        {
            // OsFlagBridge.Instance.HorizontalSpeed;
            _ui.AppendLine("Acceleration");
            FlightCapability flightCapability = OsProcessBridge.Instance.GetServices(typeof(FlightCapability)).OfType<FlightCapability>().Single();
            PhysicsValue<float> forceNeededToMaintainSpeed = flightCapability.Info.ShipGravity;
            PhysicsValue<float> maxThrustUp = flightCapability.Info.MaxThrustPerDirection[Vector3I.Down];
            float percentThrust = ((float)forceNeededToMaintainSpeed.Value / (float)maxThrustUp.Value);
            InputDriver someController = OsProcessBridge.Instance.GetDrivers(typeof(InputDriver)).OfType<InputDriver>().First();
            Vector3D naturalGravity = someController.GetNaturalGravity();
            _ui.AppendLine($"Gravity:x({naturalGravity.X,4:N})y({naturalGravity.Y,4:N})z({naturalGravity.Z,4:N})");
            _ui.AppendLine($"Gravity:x({naturalGravity.Normalized().X,4:N})y({naturalGravity.Normalized().Y,4:N})z({naturalGravity.Normalized().Z,4:N})");
            _ui.AppendLine($"ThrustPerDirection:{Environment.NewLine}" +
                           $"up     ({flightCapability.Info.ThrustPerDirection[Vector3I.Down].Value,15:N}){Environment.NewLine}" +
                           $"forward({flightCapability.Info.ThrustPerDirection[Vector3I.Backward].Value,15:N}){Environment.NewLine}" +
                           $"left   ({flightCapability.Info.ThrustPerDirection[Vector3I.Right].Value,15:N})");
            _ui.AppendLine($"{forceNeededToMaintainSpeed.ToString()} vs {maxThrustUp.ToString()}");
            if (OsFlagBridge.Instance.VerticalSpeed.HasValue)
            {
                int verticalSpeed = OsFlagBridge.Instance.VerticalSpeed.Value;
                _previousValue = verticalSpeed;
                MaintainVerticalAcceleration(percentThrust);
                _ui.AppendLine(verticalSpeed.ToString());
            }
            else if (_previousValue != null)
            {
                _previousValue = null;
                SetThrusterToPercent(Vector3I.Down, 0);
            }
        }

        private void MaintainVerticalAcceleration(float percentThrust)
        {

            bool accelerate = false;
            if (false)
            {
                //remove safe percent
            }
            _ui.AppendLine($"Acceleration set to: {percentThrust}");
            SetThrusterToPercent(Vector3I.Down, percentThrust);
        }

        private void SetThrusterToPercent(Vector3I direction, float percentThrust)
        {
            foreach (ThrustDriver thrust in OsProcessBridge.Instance.GetDrivers(typeof(ThrustDriver)).OfType<ThrustDriver>().Where(t => t.Direction == direction))
            {
                thrust.SetThrustPercent(percentThrust);
            }
        }

        public override void SetUp()
        {
            _ui = OsUiBridge.Instance.GetUiComponent(this);
        }
        
        public override AccelarationInformation Info { get; }
    }

    public struct AccelarationInformation
    {
    }
}