using System;
using System.Collections.Generic;
using IngameScript;
using VRageRender;

namespace IngameScript
{
    /**
     * This service should interact with drivers s.t. drivers can delegate the task of scanning their Blocks CustomData
     * to this service. This service then needs to notify the drivers of changes if any.
     * For that the Driver asks the OS "U got Service?" and then registers itself with this service, IFF it expects updates.
     */
    public class ScanCustomDataService : GridServiceBase
    {
        private readonly Dictionary<long, String> _settingsByEntityId = new Dictionary<long, String>();
        private OsGridAccessBridge Grid = OsGridAccessBridge.Instance;

        public override void Run()
        {
            foreach (var entityId in _settingsByEntityId.Keys)
            {
                var readDisplaySetting = Grid.ReadDisplaySetting(entityId);
                LOGGER.Info($"Updating {entityId} with {readDisplaySetting}||");
                _settingsByEntityId[entityId] = readDisplaySetting;
            }
        }

        public override List<string> GetRequirements() => new List<string>();

        public override void SetUp()
        {
            _settingsByEntityId.Clear();
        }

        public String GetUpdates(long entityId)
        {
            return _settingsByEntityId.GetValueOrDefault(entityId);
        }
    }
}
