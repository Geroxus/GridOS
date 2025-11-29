using System;
using System.Linq;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public static class DriverFactory
    {
        private static readonly ProcessIdProvider ProcessIdProvider = new ProcessIdProvider();

        private static IGridDriver Create<TDriver, TComp>(TComp enrichedComponent) 
            where TDriver : GridDriverBase<TComp>, new()
            where TComp : IEnrichedComponent
        {
            TDriver driver = new TDriver();
            string name = $"{typeof(TDriver).Name}[[{enrichedComponent.Name}]]";
            driver.Initialize(ProcessIdProvider, name);
            driver.SetComponent(enrichedComponent);
            return driver;
        }
        public static Func<IEnrichedComponent, IGridDriver> Get<TComp>() where TComp : IEnrichedComponent
        {
            if (typeof(TComp) == typeof(EnrichedTextSurface))
            {
                return (component) =>
                {
                    EnrichedTextSurface surface = component as EnrichedTextSurface;
                    return Create<DisplayDriver, EnrichedTextSurface>(surface);
                };
            }
            if (typeof(TComp) == typeof(EnrichedShipController))
            {
                return (component) =>
                {
                    EnrichedShipController controller = component as EnrichedShipController;
                    return Create<InputDriver, EnrichedShipController>(controller);
                };
            }
            if (typeof(TComp) == typeof(EnrichedThrust))
            {
                return (component) =>
                {
                    EnrichedThrust thrust = component as EnrichedThrust;
                    return Create<ThrustDriver, EnrichedThrust>(thrust);
                };
            }

            throw new Exception("not implemented");
        }
    }
}