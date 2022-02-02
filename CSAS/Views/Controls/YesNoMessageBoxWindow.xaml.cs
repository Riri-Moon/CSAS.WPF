using System.Windows;
using Prism.Mvvm;

namespace CSAS.Views.Controls
{
	/// <summary>
	/// Interaction logic for MessageBoxWindow.xaml
	/// </summary>
	public partial class YesNoMessageBoxWindow : Window
	{
		public YesNoMessageBoxWindow(bool isOkBtn)
		{
			InitializeComponent();
			if (Application.Current == null) _ = new Application();
			Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
			if (isOkBtn)
			{
				noBtn.Visibility = Visibility.Collapsed;
				yesBtn.Visibility = Visibility.Collapsed;
				okBtn.Visibility = Visibility.Visible;
			}
			else
			{
				noBtn.Visibility = Visibility.Visible;
				yesBtn.Visibility = Visibility.Visible;
				okBtn.Visibility = Visibility.Collapsed;
			}
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
		{
			this.MouseDown += delegate { DragMove(); };
		}
	}
}
