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
		protected override void OnStartup(StartupEventArgs e)
		{
			_logger.SetConfiguration(LogTargets.SingleFile, new LogConfiguration
			{
				LogToLevel = LogType.Info,
				MediaName = @"C:\CSAS",
				MediaRecord = "Log_CSAS_",
				MediaSize = 500,
				UseUtcTime = true
			});

			Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
			DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
		}
		static Logger _logger = new();
		private void APP_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			_logger.ErrorAsync(e.Exception.Message);
			_logger.ErrorAsync(e.Exception.StackTrace);

			e.Handled = true;
		}

		void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			_logger.ErrorAsync(e.Exception.Message);
			_logger.ErrorAsync(e.Exception.StackTrace);

			e.Handled = true;
		}

		void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			_logger.ErrorAsync(e.Exception.Message);
			_logger.ErrorAsync(e.Exception.StackTrace);

			e.Handled = true;
		}

		void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			_logger.ErrorAsync(e.ExceptionObject.ToString());

			var isTerminating = e.IsTerminating;
		}
	}
}
