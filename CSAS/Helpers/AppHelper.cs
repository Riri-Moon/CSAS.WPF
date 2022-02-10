using Squirrel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace CSAS.Helpers
{
	public static class AppHelper
	{
		private static readonly Logger _logger = new();
		public static async void LogUserDetails(UnitOfWork Work)
		{
			using var client = new HttpClient();
			var ip = await new HttpClient().GetStringAsync("https://api.ipify.org");
			var resp = await client.GetStringAsync($"http://ip-api.com/json/{ip}");
			var name = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();

			var userInfo = JsonSerializer.Deserialize<UserInfo>(resp);

			userInfo.Time = DateTime.Now;
			userInfo.MachineName = name;
			Work.UserInfo.Add(userInfo);
			Work.Complete();
		}
		public static void RestartApp()
		{
			System.Windows.Forms.Application.Restart();
			Application.Current.Shutdown();
		}

		public static async void CheckForUpdates()
		{
			try
			{
				using UpdateManager updateManager = await UpdateManager.GitHubUpdateManager(@"https://github.com/Riri-Moon/CSAS.WPF");
				var isUpdate = await updateManager.CheckForUpdate();

				if (isUpdate != null && isUpdate.ReleasesToApply != null && isUpdate.ReleasesToApply.Any())
				{
					if (MessageBoxHelper.Show("Update", "Je k dispozícii nový update", true).Value)
					{						
						var releaseEntry = await updateManager.UpdateApp();
						_logger.InfoAsync("Version has been updated");
					}

					if (MessageBoxHelper.Show("Reštart aplikácie", "Bol nainštalovaný update.", true).Value)
					{
						RestartApp();
					}
				}
			}
			catch (Exception ex)
			{
				_logger.InfoAsync(ex.Message);
				_logger.ErrorAsync(ex.StackTrace);
			}
		}
	}
}
