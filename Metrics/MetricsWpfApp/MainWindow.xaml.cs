using LiveCharts.Wpf;
using LiveCharts;
using MetricsManager.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MetricsWpfApp.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using LiveCharts.Wpf.Charts.Base;

namespace MetricsWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppResources _appResources;
        private AgentInfo _selectedAgent;


        public MainWindow()
        {
            _appResources = new AppResources();

            InitializeComponent();

            AgentSelectPanel.Visibility = Visibility.Collapsed;
            MetricSelectPanel.Visibility = Visibility.Collapsed;

            AgentCombo.ItemsSource = _appResources.GetAgentsName();
            MetricCombo.ItemsSource = _appResources.GetMetricsName();
            
            if (AgentCombo.SelectedIndex > 1) MetricSelectPanel.Visibility = Visibility.Visible;
        }

        #region Manager Configuration

        private void ManagerUri_GotFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ManagerUri.Text) || String.Equals("http://[address]:[port]", ManagerUri.Text))
            {
                ManagerUri.Text = "http://";
            }
        }

        private void ManagerUri_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(AddAgentUri.Text) || String.Equals("http://", AddAgentUri.Text))
            {
                ManagerUri.Text = "http://[address]:[port]";
            }
        }

        private async void GetAgents_Click(object sender, RoutedEventArgs e)
        {
            AgentSelectPanel.Visibility = Visibility.Collapsed;
            MetricSelectPanel.Visibility = Visibility.Collapsed;

            _appResources.DeleteAllAgents();
            AgentCombo.Text = "Выберите агента";
            AgentCombo.SelectedIndex = -1;
            AgentSelectedURI.Text = $"URI:";
            AgentSelectedStatus.Text = $"Статус:";

           

            if (String.IsNullOrEmpty(AddAgentUri.Text) == false && Uri.IsWellFormedUriString(ManagerUri.Text, UriKind.RelativeOrAbsolute)) 
            {
                MetricsAgentClient metricsAgentClient = new MetricsAgentClient(ManagerUri.Text, new System.Net.Http.HttpClient());

                if (metricsAgentClient != null)
                {
                    try
                    {
                        UpdateAgentList(await metricsAgentClient.GetallAsync());
                        AgentSelectPanel.Visibility = Visibility.Visible;
                    }
                    catch (Exception)
                    {
                        AgentSelectPanel.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        #endregion

        #region Agent Select / Add
        private void AgentCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                AgentCombo.ItemsSource = _appResources.GetAgentsName();
                AgentCombo.SelectedIndex = -1;
                AgentSelectedURI.Text = $"URI:";
                AgentSelectedStatus.Text = $"Статус:";
            }
            else if (uint.TryParse(e.AddedItems[0]?.ToString(), out uint agentId))
            {
                _selectedAgent = _appResources.GetAgentById(agentId);
                AgentSelectedURI.Text = $"URI: {_selectedAgent.AgentAddress}";
                string status = _selectedAgent.Enable == true ? "Активен" : "Остановлен";
                AgentSelectedStatus.Text = $"Статус: {status}";
                MetricSelectPanel.Visibility = _selectedAgent == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void AddAgentPanelShowButton_Click(object sender, RoutedEventArgs e)
        {
            AddAgentPanel.Visibility = AddAgentPanel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void AddAgentUri_GotFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(AddAgentUri.Text) || String.Equals("http://[address]:[port]", AddAgentUri.Text))
            {
                AddAgentUri.Text = "http://";
            }
        }

        private void AddAgentUri_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(AddAgentUri.Text) || String.Equals("http://", AddAgentUri.Text))
            {
                AddAgentUri.Text = "http://[address]:[port]";
            }
        }

        private void AddAgentId_GotFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(AddAgentId.Text) || String.Equals("ID агента", AddAgentId.Text))
            {
                AddAgentId.Text = "";
            }
        }

        private void AddAgentId_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(AddAgentId.Text))
            {
                AddAgentId.Text = "ID агента";
            }

            if (uint.TryParse(AddAgentId.Text, out uint agentId) == false)
            {
                AddAgentId.Text = "ID агента";
            }
        }

        private void CancelAgentAddButton_Click(object sender, RoutedEventArgs e)
        {
            AddAgentPanel.Visibility = Visibility.Collapsed;
            AddAgentUri.Text = "http://[address]:[port]";
        }
        
        private async void ConfirmAgentAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(AddAgentUri.Text) || String.Equals("http://", AddAgentUri.Text))
            {
                return;
            }

            if (uint.TryParse(AddAgentId.Text, out uint agentId))
            {
                AgentInfo agentInfo = new AgentInfo();

                agentInfo.AgentId = agentId;

                try
                {
                    Uri agentUri = new Uri(AddAgentUri.Text);

                    if (agentUri.IsAbsoluteUri == false)
                    {
                        return;
                    }

                    if (String.IsNullOrEmpty(ManagerUri.Text) == false)
                    {
                        agentInfo.AgentAddress = agentUri;
                        agentInfo.Enable = (bool)AddAgentActiveStatus.IsChecked;

                        MetricsAgentClient metricsAgentClient = new MetricsAgentClient(ManagerUri.Text, new System.Net.Http.HttpClient());

                        await metricsAgentClient.RegisterAsync(agentInfo);

                        UpdateAgentList(await metricsAgentClient.GetallAsync());

                        AddAgentPanel.Visibility = Visibility.Collapsed;
                    }
                }
                catch (Exception)
                {

                }
 
            }
        }

        private void UpdateAgentList(ICollection<AgentInfo> agents)
        {
            if (agents != null && agents.Count > 0)
            {
                foreach (AgentInfo agent in agents)
                {
                    _appResources.AddAgent(agent);
                }

                AgentCombo.ItemsSource = _appResources.GetAgentsName();
            }
        }

        #endregion

        #region Update Chart Data

        private async void GetMetricDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAgent == null)
            {
                return;
            }


            switch (MetricCombo.SelectedIndex)
            {
                case 0:
                    {
                        Chart.UpdateChart(await this.GetCpuMetrics(int.Parse(AgentCombo.Text), new CpuMetricsClient(ManagerUri.Text, new System.Net.Http.HttpClient())));
                    }
                    break;
                case 1:
                    {
                        Chart.UpdateChart(await this.GetDotNetMetrics(int.Parse(AgentCombo.Text), new DotNetMetricsClient(ManagerUri.Text, new System.Net.Http.HttpClient())));
                    }
                    break;
                case 2:
                    {
                        Chart.UpdateChart(await this.GetHddMetrics(int.Parse(AgentCombo.Text), new HddMetricsClient(ManagerUri.Text, new System.Net.Http.HttpClient())));
                    }
                    break;
                case 3:
                    {
                        Chart.UpdateChart(await this.GetNetworkMetrics(int.Parse(AgentCombo.Text), new NetworkMetricsClient(ManagerUri.Text, new System.Net.Http.HttpClient())));
                    }
                    break;
                case 4:
                    {
                        Chart.UpdateChart(await this.GetRamMetrics(int.Parse(AgentCombo.Text), new RamMetricsClient(ManagerUri.Text, new System.Net.Http.HttpClient())));
                    }
                    break;
                default:
                    break;
            }
        }

        public async Task<SeriesCollection> GetCpuMetrics(int agentId, CpuMetricsClient metricsClient)
        {
            if (metricsClient == null)
            {
                return null;
            }

            try
            {
                ChartDescription chartDescription = new ChartDescription();
                chartDescription.ChartName = $"{MetricCombo.Text} (%)";

                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

                CpuMetricsResponse response = await metricsClient.ToGetAsync(
                    agentId,
                    fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                    toTime.ToString("dd\\.hh\\:mm\\:ss"));

                if (response == null) return null;

                if (response.Metrics.Count > 0)
                {

                    chartDescription.DescriptionName = $"За последние {TimeSpan.FromSeconds(response.Metrics.ToArray()[response.Metrics.Count - 1].Time - response.Metrics.ToArray()[0].Time)} средняя загрузка";

                    chartDescription.DescritionData = $"{response.Metrics.Where(x => x != null).Select(x => x.Value).ToArray().Sum(x => x) / response.Metrics.Count:F2} %";
                }

                SeriesCollection ColumnSeriesValues = new SeriesCollection
                {
                     new LineSeries
                    {
                        Values = new ChartValues<int>(response.Metrics.Where(x => x != null).Select(x => (int)x.Value).ToArray())
                    },
                    new ColumnSeries
                    {
                        Values = new ChartValues<int>(response.Metrics.Where(x => x != null).Select(x => (int)x.Value).ToArray())
                    }
                };

                Chart.UpdateChartDescription(chartDescription);
                return ColumnSeriesValues;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<SeriesCollection> GetDotNetMetrics(int agentId, DotNetMetricsClient metricsClient)
        {
            if (metricsClient == null)
            {
                return null;
            }

            try
            {
                ChartDescription chartDescription = new ChartDescription();
                chartDescription.ChartName = $"{MetricCombo.Text} (шт)";

                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

                DotNetMetricsResponse response = await metricsClient.ToGetAsync(
                    agentId,
                    fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                    toTime.ToString("dd\\.hh\\:mm\\:ss"));

                if (response == null) return null;

                if (response.Metrics.Count > 0)
                {

                    chartDescription.DescriptionName = $"За последние {TimeSpan.FromSeconds(response.Metrics.ToArray()[response.Metrics.Count - 1].Time - response.Metrics.ToArray()[0].Time)} среднее количество ошибок";

                    chartDescription.DescritionData = $"{response.Metrics.Where(x => x != null).Select(x => x.Value).ToArray().Sum(x => x) / response.Metrics.Count:F0} шт";
                }


                SeriesCollection ColumnSeriesValues = new SeriesCollection
                {
                     new LineSeries
                    {
                        Values = new ChartValues<float>(response.Metrics.Where(x => x != null).Select(x => (float)x.Value).ToArray())
                    },
                     new ColumnSeries
                    {
                        Values = new ChartValues<int>(response.Metrics.Where(x => x != null).Select(x => (int)x.Value).ToArray())
                    }
                };

                Chart.UpdateChartDescription(chartDescription);
                return ColumnSeriesValues;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<SeriesCollection> GetHddMetrics(int agentId, HddMetricsClient metricsClient)
        {
            if (metricsClient == null)
            {
                return null;
            }

            try
            {
                ChartDescription chartDescription = new ChartDescription();
                chartDescription.ChartName = $"{MetricCombo.Text} (%)";

                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

                HddMetricsResponse response = await metricsClient.ToGetAsync(
                    agentId,
                    fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                    toTime.ToString("dd\\.hh\\:mm\\:ss"));

                if (response == null) return null;

                if (response.Metrics.Count > 0)
                {

                    chartDescription.DescriptionName = $"За последние {TimeSpan.FromSeconds(response.Metrics.ToArray()[response.Metrics.Count - 1].Time - response.Metrics.ToArray()[0].Time)} средняя загрузка";

                    chartDescription.DescritionData = $"{response.Metrics.Where(x => x != null).Select(x => x.Value).ToArray().Sum(x => x) / response.Metrics.Count:F2}  %";
                }


                SeriesCollection ColumnSeriesValues = new SeriesCollection
                {
                     new LineSeries
                    {
                        Values = new ChartValues<float>(response.Metrics.Where(x => x != null).Select(x => (float)x.Value).ToArray())
                    },
                    new ColumnSeries
                    {
                        Values = new ChartValues<int>(response.Metrics.Where(x => x != null).Select(x => (int)x.Value).ToArray())
                    }
                };

                Chart.UpdateChartDescription(chartDescription);
                return ColumnSeriesValues;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<SeriesCollection> GetNetworkMetrics(int agentId, NetworkMetricsClient metricsClient)
        {
            if (metricsClient == null)
            {
                return null;
            }

            try
            {
                ChartDescription chartDescription = new ChartDescription();
                chartDescription.ChartName = $"{MetricCombo.Text} (байт/c)";

                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

                NetworkMetricsResponse response = await metricsClient.ToGetAsync(
                    agentId,
                    fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                    toTime.ToString("dd\\.hh\\:mm\\:ss"));

                if (response == null) return null;

                if (response.Metrics.Count > 0)
                {

                    chartDescription.DescriptionName = $"За последние {TimeSpan.FromSeconds(response.Metrics.ToArray()[response.Metrics.Count - 1].Time - response.Metrics.ToArray()[0].Time)} средняя загрузка";

                    chartDescription.DescritionData = $"{response.Metrics.Where(x => x != null).Select(x => x.Value).ToArray().Sum(x => x) / response.Metrics.Count:F0} байт/c";
                }


                SeriesCollection ColumnSeriesValues = new SeriesCollection
                {
                     new LineSeries
                    {
                        Values = new ChartValues<float>(response.Metrics.Where(x => x != null).Select(x => (float)x.Value).ToArray())
                    },
                     new ColumnSeries
                    {
                        Values = new ChartValues<int>(response.Metrics.Where(x => x != null).Select(x => (int)x.Value).ToArray())
                    }
                };

                Chart.UpdateChartDescription(chartDescription);
                return ColumnSeriesValues;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<SeriesCollection> GetRamMetrics(int agentId, RamMetricsClient metricsClient)
        {
            if (metricsClient == null)
            {
                return null;
            }

            try
            {
                ChartDescription chartDescription = new ChartDescription();
                chartDescription.ChartName = $"{MetricCombo.Text} (%)";

                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

                RamMetricsResponse response = await metricsClient.ToGetAsync(
                    agentId,
                    fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                    toTime.ToString("dd\\.hh\\:mm\\:ss"));

                if (response == null) return null;

                if (response.Metrics.Count > 0)
                {

                    chartDescription.DescriptionName = $"За последние {TimeSpan.FromSeconds(response.Metrics.ToArray()[response.Metrics.Count - 1].Time - response.Metrics.ToArray()[0].Time)} средняя загрука памяти";

                    chartDescription.DescritionData = $"{response.Metrics.Where(x => x != null).Select(x => x.Value).ToArray().Sum(x => x) / response.Metrics.Count:F2} %";
                }


                SeriesCollection ColumnSeriesValues = new SeriesCollection
                {
                     new LineSeries
                    {
                        Values = new ChartValues<float>(response.Metrics.Where(x => x != null).Select(x => (float)x.Value).ToArray())
                    },
                    new ColumnSeries
                    {
                        Values = new ChartValues<int>(response.Metrics.Where(x => x != null).Select(x => (int)x.Value).ToArray())
                    }
                };

                Chart.UpdateChartDescription(chartDescription);
                return ColumnSeriesValues;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #endregion

    }
}
