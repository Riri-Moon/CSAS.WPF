using CSAS.ViewModels;
using System.Windows;

namespace CSAS.Views
{
	/// <summary>
	/// Interaction logic for MainGroup.xaml
	/// </summary>
	public partial class MainGroupView : Window
	{
		public MainGroupView()
		{
			InitializeComponent();
			DataContext = new MainGroupViewModel();
		}
	}
}
