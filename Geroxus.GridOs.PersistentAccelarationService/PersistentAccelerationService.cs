using System;
using System.Linq;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public class PersistentAccelerationService : GridServiceBase<AccelarationInformation>
    {
        private StringBuilder _ui;

        public override void Run()
        {
            // OsFlagBridge.Instance.HorizontalSpeed;
            _ui.AppendLine("Acceleration");
            if (OsFlagBridge.Instance.VerticalSpeed.HasValue)
            {
                int verticalSpeed = OsFlagBridge.Instance.VerticalSpeed.Value;
                MaintainVerticalAcceleration(verticalSpeed);
                _ui.AppendLine(verticalSpeed.ToString());
            }
        }

        private void MaintainVerticalAcceleration(int targetSpeedVertical)
        {
            FlightCapability flightCapability = OsProcessBridge.Instance.GetServices(typeof(FlightCapability)).OfType<FlightCapability>().Single();
            PhysicsValue<float> forceNeededToMaintainSpeed = flightCapability.Info.ShipGravity;
            PhysicsValue<float> maxThrustUp = flightCapability.Info.MaxThrustPerDirection[Vector3I.Down];
            float percentThrust = ((float)forceNeededToMaintainSpeed.Value / (float)maxThrustUp.Value);
            InputDriver someController = OsProcessBridge.Instance.GetDrivers(typeof(InputDriver)).OfType<InputDriver>().First();
            Vector3D naturalGravity = someController.GetNaturalGravity();
            _ui.AppendLine($"Gravity:{Environment.NewLine}" +
                           $"x({naturalGravity.X:N}){Environment.NewLine}" +
                           $"y({naturalGravity.Y:N}){Environment.NewLine}" +
                           $"z({naturalGravity.Z:N})");
            _ui.AppendLine($"ThrustPerDirection:{Environment.NewLine}" +
                           $"up({flightCapability.Info.MaxThrustPerDirection[Vector3I.Down].Value :N})");
            _ui.AppendLine(
                $"Current Thrust: {flightCapability.Info.ThrustPerDirection[Vector3I.Down].Value :N}");
            _ui.AppendLine($"{forceNeededToMaintainSpeed.ToString()} vs {maxThrustUp.ToString()}");
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