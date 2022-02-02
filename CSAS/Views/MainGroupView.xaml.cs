using CSAS.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

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
		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			var start = new ProcessStartInfo(e.Uri.AbsoluteUri)
			{
				UseShellExecute = true
			};
			Process.Start(start);
			e.Handled = true;
		}
	}
}
