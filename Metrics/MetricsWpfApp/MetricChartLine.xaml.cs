using LiveCharts;
using MetricsWpfApp.Models;
using System.ComponentModel;
using System.Windows.Controls;


namespace MetricsWpfApp
{
    /// <summary>
    /// Interaction logic for MetricChartLine.xaml
    /// </summary>
    public partial class MetricChartLine : UserControl, INotifyPropertyChanged
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

        public MetricChartLine()
        {
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
