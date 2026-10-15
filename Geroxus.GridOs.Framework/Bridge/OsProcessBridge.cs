using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace IngameScript
{
    public class OsProcessBridge : OsBridge<OsProcessBridge>
    {
        private Dictionary<ProcessId, IGridOsProcess> _processes;
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
            // TODO find a better solution.
            // using "if" leads to it taking several minutes until anything is being displayed
            while (_processesToRegister.Count > 0)
            {
                IGridOsProcess process;
                process = _processesToRegister.Dequeue();
                AttemptStart(process);
            }

            if (_processesForTermination.Count == 0) return;
            var processId = _processesForTermination.Dequeue();
            LOGGER.Info($"Stopping [[{processId}]]");
            Stop(processId);
        }

        private void AttemptStart(IGridOsProcess process)
        {
            try
            {
                foreach (String req in process.GetRequirements())
                    // ReSharper disable once SimplifyLinqExpressionUseAll
                    if (!_processes.Values.Any(p => p.Name == req))
                        throw new RequirementsNotMetException(process.Name, req);
                process.SetUp();
                _processes.Add(process.ProcessId, process);
            }
            catch (RequirementsNotMetException e)
            {
                // This effectively silences the Error. Meh, need a better system for this kinda thing.
                LOGGER.Error(e.Message);
                _processesToRegister.Enqueue(process);
            }
        }

        public void RegisterStop(ProcessId processId)
        {
            _processesForTermination.Enqueue(processId);
        }

        private void Stop(ProcessId processId)
        {
            List<IGridOsProcess> gridOsProcesses = GetAllProcesses().Where(p => p.ProcessId.Equals(processId)).ToList();
            if (gridOsProcesses.Count() != 1) return;
            IGridOsProcess processToDispose = gridOsProcesses.Single();

            LOGGER.Info("Stop Process: " + processToDispose.Name);
            processToDispose.Dispose();
            _processes.Remove(processToDispose.ProcessId);
        }
    }

    public class RequirementsNotMetException : Exception
    {
        public RequirementsNotMetException(String process, String requirement)
            : base($"Creating: {process} misses requirement: {requirement}")
        {
        }
    }
}