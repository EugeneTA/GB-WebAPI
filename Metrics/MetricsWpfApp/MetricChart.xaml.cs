using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf;
using MetricsManager.Client;
using MetricsWpfApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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

namespace MetricsWpfApp
{
    /// <summary>
    /// Interaction logic for CpuChart.xaml
    /// </summary>
    public partial class MetricChart : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private SeriesCollection _columnSeriesValues;

        public SeriesCollection ColumnSeriesValues
        {
            get
            {
                return _columnSeriesValues;
            }
            set
            {
                _columnSeriesValues = value;
                OnPropertyChanged("ColumnSeriesValues");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public MetricChart()
        {
            _columnSeriesValues = new SeriesCollection();
            InitializeComponent();
            DataContext = this;
        }

        public void UpdateChart(SeriesCollection series)
        {
            if (series == null) return;

            ColumnSeriesValues = series;
            TimePowerChart.Update(true);
        }

        
        public void UpdateChartDescription(ChartDescription chartDescription)
        {
            if (chartDescription == null) return;

            ChartNameBlock.Text = chartDescription.ChartName == null ? "" : chartDescription.ChartName;
            PercentDescriptionTextBlock.Text = chartDescription.DescriptionName == null ? "" : chartDescription.DescriptionName;
            PercentTextBlock.Text = chartDescription.DescritionData == null ? "" : chartDescription.DescritionData;

        }

    }


}
