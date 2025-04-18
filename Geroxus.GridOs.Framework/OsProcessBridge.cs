using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace IngameScript
{
    public class OsProcessBridge : OsBridge<OsProcessBridge>
    {
        private Dictionary<ProcessId, IGridDriver> _drivers;
        private Dictionary<ProcessId, IGridService> _services;
        private Queue<ProcessId> _processesForTermination = new Queue<ProcessId>();

        public void RegisterProcessLists(Dictionary<ProcessId, IGridDriver> drivers, Dictionary<ProcessId, IGridService> services)
        {
            _drivers = drivers;
            _services = services;
        }

        public ImmutableList<IGridDriver> GetDrivers(Type type)
        {
            return _drivers.Values.Where(d => d.GetType() == type).ToImmutableList();
        }

        public ImmutableList<IGridOsProcess> GetAllProcesses()
        {
            ImmutableList<IGridOsProcess> gridOsProcesses = _drivers.Values
                .Select(it => it as IGridOsProcess)
                .Union(_services.Values.Select(it => it as IGridOsProcess))
                .ToImmutableList();
            LOGGER.Write($"Getting all [[{gridOsProcesses.Count}]] grid processes");
            return gridOsProcesses;
        }

        public void Register(IGridOsProcess process)
        {
            if (process == null) return;
            if (process is IGridDriver)
                _drivers.Add(process.ProcessId, process as IGridDriver);
            else if (process is IGridService)
                _services.Add(process.ProcessId, process as IGridService);
        }

        public void RegisterStop(ProcessId processId)
        {
            _processesForTermination.Enqueue(processId);
        }

        public void StopNext()
        {
            if (_processesForTermination.Count == 0) return;
            var processId = _processesForTermination.Dequeue();
            LOGGER.Write($"Stopping [[{processId}]]");
            Stop(processId);
        }

        public void Stop(ProcessId processId)
        {
            LOGGER.Write("Stop in Process: " + processId);
            List<IGridOsProcess> gridOsProcesses = GetAllProcesses().Where(p => p.ProcessId.Equals(processId)).ToList();
            if (gridOsProcesses.Count() != 1) return;
            IGridOsProcess processToDispose = gridOsProcesses.Single();
            
            LOGGER.Write("Stop in Process: " + processToDispose.Name);
            processToDispose.Dispose();
            if (processToDispose is IGridDriver)
                _drivers.Remove(processToDispose.ProcessId);
            else if (processToDispose is IGridService)
                _services.Remove(processToDispose.ProcessId);
        }
    }
}