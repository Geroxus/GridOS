using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace IngameScript
{
    public class OsProcessBridge : OsBridge<OsProcessBridge>
    {
        private Dictionary<ProcessId,IGridOsProcess> _processes;
        private readonly Queue<ProcessId> _processesForTermination = new Queue<ProcessId>();
        private readonly Queue<IGridOsProcess> _processesToRegister = new Queue<IGridOsProcess>();

        public void RegisterProcessLists(Dictionary<ProcessId, IGridOsProcess> processes)
        {
            _processes = processes;
        }

        public ImmutableList<IGridDriver> GetDrivers(Type type = null)
        {
            return _processes.Values
                .OfType<IGridDriver>()
                .Where(d => type == null || d.GetType() == type)
                .ToImmutableList();
        }

        public ImmutableList<IGridService> GetServices(Type type = null)
        {
            return _processes.Values
                .OfType<IGridService>()
                .Where(s => type == null || s.GetType() == type)
                .ToImmutableList();
        }

        public ImmutableList<IGridOsProcess> GetAllProcesses()
        {
            return _processes.Values.ToImmutableList();
        }

        public void Register(IGridOsProcess process)
        {
            if (process == null) return;
            _processesToRegister.Enqueue(process);
        }

        public void Run()
        {
            while (_processesToRegister.Count > 0)
            {
                var process = _processesToRegister.Dequeue();
                _processes.Add(process.ProcessId, process);
            }
            
            if (_processesForTermination.Count == 0) return;
            var processId = _processesForTermination.Dequeue();
            LOGGER.Write($"Stopping [[{processId}]]");
            Stop(processId);
        }

        public void RegisterStop(ProcessId processId)
        {
            _processesForTermination.Enqueue(processId);
        }

        private void Stop(ProcessId processId)
        {
            LOGGER.Write("Stop in Process: " + processId);
            List<IGridOsProcess> gridOsProcesses = GetAllProcesses().Where(p => p.ProcessId.Equals(processId)).ToList();
            if (gridOsProcesses.Count() != 1) return;
            IGridOsProcess processToDispose = gridOsProcesses.Single();
            
            LOGGER.Write("Stop in Process: " + processToDispose.Name);
            processToDispose.Dispose();
            _processes.Remove(processToDispose.ProcessId);
        }
    }
}