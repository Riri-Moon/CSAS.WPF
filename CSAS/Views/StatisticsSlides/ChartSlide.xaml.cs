using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Windows;
using System.Windows.Controls;

namespace CSAS.Views.StatisticsSlides
{
	/// <summary>
	/// Interaction logic for ChartSlide.xaml
	/// </summary>
	public partial class ChartSlide : UserControl
	{
		public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(nameof(ChartSeries), typeof(List<ISeries>), typeof(ChartSlide));
		public static readonly DependencyProperty LabelsProperty = DependencyProperty.Register(nameof(Labels), typeof(List<Axis>), typeof(ChartSlide));

		public List<ISeries> ChartSeries
		{
			get => (List<ISeries>)GetValue(SeriesProperty);
			set => SetValue(SeriesProperty, value);
		}

		public List<Axis> Labels
		{
			get => (List<Axis>)GetValue(LabelsProperty);
			set => SetValue(LabelsProperty, value);
		}
		public ChartSlide()
		{
			InitializeComponent();
		}
	}
}
