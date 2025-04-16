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

        public List<IGridDriver> Get(Func<IMyTextSurface, IGridDriver> func)
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
                        result.Add(func(myTextSurfaceProvider.GetSurface(i)));
                }

                myTerminalBlock.CustomData = Ini.ToString();
            }
            return result;
        }
    }
}