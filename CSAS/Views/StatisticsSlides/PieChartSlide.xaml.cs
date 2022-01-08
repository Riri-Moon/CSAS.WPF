using LiveChartsCore;
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

namespace CSAS.Views.StatisticsSlides
{
	/// <summary>
	/// Interaction logic for PieChartSlide.xaml
	/// </summary>
	
	public partial class PieChartSlide : UserControl
	{
		public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(nameof(ChartSeries), typeof(List<ISeries>), typeof(PieChartSlide));
		public List<ISeries> ChartSeries
		{
			get => (List<ISeries>)GetValue(SeriesProperty);
			set => SetValue(SeriesProperty, value);
		}
		public PieChartSlide()
		{
			InitializeComponent();
		}
	}
}
