using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    internal class OsGridAccessBridge : OsBridge<OsGridAccessBridge>
    {
        private IMyGridTerminalSystem _gridTerminalSystem;
        private MyIni Ini { get; } = new MyIni();

        public void RegisterGridTerminalSystem(IMyGridTerminalSystem gridTerminalSystem)
        {
            if (_gridTerminalSystem != null)
                throw new Exception("Do not call RegisterGridTerminalSystem more than once");
            _gridTerminalSystem = gridTerminalSystem;
        }

        public List<IGridDriver> Get<T>(Func<IEnrichedComponent<T>, IGridDriver> factory)
        {
            if (typeof(T) == typeof(IMyTextSurface))
                return GetDisplayDrivers(factory);
            else if (typeof(T) == typeof(IMyShipController))
                return GetInputDrivers(factory);
            
            throw new Exception($"Type '{typeof(T).Name}' is not supported");
        }

        private List<IGridDriver> GetInputDrivers<T>(Func<IEnrichedComponent<T>,IGridDriver> factory)
        {
            List<IMyShipController> shipControllers = new List<IMyShipController>();
            _gridTerminalSystem.GetBlocksOfType(shipControllers);
            
            List<IGridDriver> result = new List<IGridDriver>();
            foreach (IMyShipController shipController in shipControllers)
            {
                Ini.TryParse(shipController.CustomData);
                if (!Ini.ContainsSection("GridOS")) continue;

                IEnrichedComponent<T> enrichedShipController = new EnrichedShipController(shipController)
                    as IEnrichedComponent<T>;
                result.Add(factory(enrichedShipController));
            }
            
            return result;
        }

        private List<IGridDriver> GetDisplayDrivers<T>(Func<IEnrichedComponent<T>, IGridDriver> factory)
        {
            List<IMyTerminalBlock> list = new List<IMyTerminalBlock>();
            _gridTerminalSystem.GetBlocksOfType(list, b => b is IMyTextSurfaceProvider);

            List<IGridDriver> result = new List<IGridDriver>();
            foreach (var myTerminalBlock in list)
            {
                Ini.TryParse(myTerminalBlock.CustomData);
                if (!Ini.ContainsSection("GridOS")) continue;

                var myTextSurfaceProvider = (IMyTextSurfaceProvider)myTerminalBlock;
                int surfaceCount = myTextSurfaceProvider.SurfaceCount;
                for (int i = 0; i < surfaceCount; i++)
                {
                    bool loadDisplay = false;
                    if (Ini.ContainsKey("GridOS", i.ToString()))
                        loadDisplay |= Ini.Get("GridOS", i.ToString()).ToBoolean();
                    else
                        Ini.Set("GridOS", i.ToString(), false);

                    if (loadDisplay)
                    {
                        IMyTextSurface myTextSurface = myTextSurfaceProvider.GetSurface(i);
                        IEnrichedComponent<T> enrichedTextSurface =
                            new EnrichedTextSurface(myTextSurface, $"{myTerminalBlock.DisplayNameText}:{i}") as
                                IEnrichedComponent<T>;
                        result.Add(factory(enrichedTextSurface));
                    }

                    ;
                }

                myTerminalBlock.CustomData = Ini.ToString();
            }

            return result;
        }
    }
}