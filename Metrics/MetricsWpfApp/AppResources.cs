using MetricsManager.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MetricsWpfApp
{
    internal class AppResources
    {
        IDictionary<uint, string> _metrics;
        IDictionary<uint, AgentInfo> _agents;

        public AppResources()
        {
            _metrics = new Dictionary<uint, string>();
            _agents = new Dictionary<uint, AgentInfo>();

            _metrics.Add(0, "Загрузка процессора");
            _metrics.Add(1, "Ошибки dotNet");
            _metrics.Add(2, "Загрузка HDD");
            _metrics.Add(3, "Загрузка сети");
            _metrics.Add(4, "Использование памяти");

            GetMetricsName();
        }

        public void DeleteAllAgents()
        {
            _agents = new Dictionary<uint, AgentInfo>();
        }
        public void AddAgent(AgentInfo agentInfo)
        {
            if (agentInfo != null)
            {
                _agents.TryAdd((uint)agentInfo.AgentId, agentInfo);
            }
        }

        public AgentInfo GetAgentById(uint agentId)
        {
            return _agents[agentId];
        }

        public ObservableCollection<string> GetMetricsName()
        {
            if (_metrics == null) return null;

            ObservableCollection<string> result = new ObservableCollection<string>();

            foreach (var metric in _metrics)
            {
                result.Add(metric.Value);
            }

            return result;
        }

        public ObservableCollection<string> GetAgentsName()
        {
            if (_agents == null) return new ObservableCollection<string>();

            ObservableCollection<string> result = new ObservableCollection<string>();

            foreach (var agent in _agents)
            {
                if (agent.Value != null) result.Add(agent.Value.AgentId.ToString());
            }

            return result;
        }

    }
}
