using IBM.Tools.Common.Helper.Logger;
using System.Windows;
using System.Windows.Threading;

namespace CSAS
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void APP_DispatcherUnhandledException(object sender,
	   DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.Message);
			e.Handled = true;
		}
	}
}
