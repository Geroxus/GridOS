using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace IngameScript
{
    public class LiveStats : GridProgramBase
    {
        private readonly List<DisplayDriver> _processListDisplays = new List<DisplayDriver>();
        private readonly List<DisplayDriver> _flightCapabilityDisplays = new List<DisplayDriver>();

        public override void Run()
        {
            // generate text to output
            TextNode processListContent = Ui.RootNode.Children[0].Children[0] as TextNode;
            if (processListContent == null)
                throw new NullReferenceException("ProcessListContent is not set!");
            processListContent.AppendLine("Welcome to LiveStats!");
            processListContent.AppendLine("Running Processes:");
            OsProcessBridge.Instance.GetAllProcesses()
                .Where(p => p.ProcessId.Id < 90000).ToImmutableList()
                .Sort((p1, p2) => p1.ProcessId.Id < p2.ProcessId.Id ? -1 : 1)
                .ForEach(p => processListContent.AppendLine($"{p.ProcessId.Id, 6 :N0}: {p.Name}"));
            int driversCount = OsProcessBridge.Instance.GetAllProcesses()
                .Count(p => p.ProcessId.Id >= 90000);
            processListContent.AppendLine($"And this many drivers: {driversCount}");

            // Flight capability doesn't get drawn right now due to restructuring things
            StringBuilder flightCapabilityContent = new StringBuilder();
            FlightCapability flightCapabilityService = OsProcessBridge.Instance.GetServices(typeof(FlightCapability)).Single() as FlightCapability;
            if (flightCapabilityService == null)
                throw new Exception("Flight capability service not found!");
            FlightCapabilityInfo flightCapabilityInfo = flightCapabilityService.Info;
            flightCapabilityContent.AppendLine("Flight Capability Information");
            flightCapabilityContent.AppendLine(
                $"Ship Mass: {flightCapabilityInfo.ShipMass.Value :N} {flightCapabilityInfo.ShipMass.Unit.Short()}");
            flightCapabilityContent.AppendLine(
                $"Force on Ship: {flightCapabilityInfo.ShipGravity.Value :N} {flightCapabilityInfo.ShipGravity.Unit.Short()}");
            flightCapabilityContent.AppendLine(
                $"Natural Gravity: {flightCapabilityInfo.NaturalGravity.Value:N} {flightCapabilityInfo.NaturalGravity.Unit.Short()}");
            flightCapabilityContent.AppendLine(
                $"Can currently sustain flight? {(flightCapabilityInfo.CurrentFlightSustain.CanSustainFlight ? "Yes" : "No")}");
            GridOsPlanet? targetPlanet = flightCapabilityInfo.TargetFlightSustain.Target;
            flightCapabilityContent.AppendLine(
                $"Can sustain flight on {targetPlanet?.Name}({targetPlanet?.Gravity.Value :N}{targetPlanet?.Gravity.Unit.Short()})? {(flightCapabilityInfo.TargetFlightSustain.CanSustainFlight ? "Yes" : "No")}");

        }

        public override void SetUp()
        {
            Ui.Program = GridProgram.LiveStats;
            Ui.RootNode.CreateChildNode<ContainerNode>()
                .AddDrawCondition(GridUiDrawCondition.WindowRequested("ProcessList"))
                .CreateChildNode<TextNode>();
            Ui.RootNode.CreateChildNode<ContainerNode>()
                .AddDrawCondition(GridUiDrawCondition.WindowRequested("FlightCapability"))
                .CreateChildNode<TextNode>();
        }
    }

}