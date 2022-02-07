using System.IO;
using System.Net.Mail;
using System.Text;

namespace CSAS.Helpers
{
	public class NotificationHelper
	{
		readonly Logger _logger = new();
		private UnitOfWork Work { get; set; }
		public Settings Settings { get; set; }
		public ExcelService Excel { get; set; }
		private OutlookService OutlookService { get; set; }

		private readonly string space = "<br/><br/>";

		public NotificationHelper()
		{
			Excel = new();
			Work = UoWSingleton.Instance;
			OutlookService = new OutlookService();
		}

		public void SendNotification()
		{
			StringBuilder builder = new();
			try 
			{
				var act = Work.Activity.GetAll().Where(x => x.IsSendNotifications);
				if (act == null || !act.Any())
				{
					return;
				}

				var grpId = act.FirstOrDefault().Student.MainGroup.Id;
				Settings = Work.Settings.GetSettingsByMainGroup(grpId);

				

				foreach (var activity in act.Where(x => (x.Deadline - DateTime.Today).TotalDays <= 3).Where(x => x.Modified <= x.Created))
				{
					var email = new MailAddressCollection
				{
					activity.Student.SchoolEmail
				};
					string subject = $"Neodovzdaná aktivita - {activity.Name}";
					builder.Append($"Dobrý deň, {space} uporňujem Vás na blížiaci sa dátum odovzdania aktivity - {activity.Deadline.ToShortDateString()} {activity.Deadline.ToShortTimeString()} zo dňa {activity.Created.ToShortDateString()}.{space}" +
						$"Ak ste aktivitu odovzdali, pokladajte tento email za bezpredmetný.");

					if (activity.Attachments != null && activity.Attachments.Count > 0)
					{
						List<string> attachments = new();

						builder.Append($"{space} V prílohe Vám posielam taktiež podklady ku aktivite.");
						foreach (var attachment in activity.Attachments)
						{
							if (File.Exists(attachment.PathToFile))
							{
								attachments.Add(attachment.PathToFile);
							}
						}
						//builder.Append($"{space} S pozdravom {Settings.Title} {Settings.Name} {Settings.TitleAfterName}");
						OutlookService.SendEmail(subject, email, null, builder.ToString(), attachments, false,Settings.Signature);
					}
					else
					{
						//builder.Append($"{space} S pozdravom {Settings.Title} {Settings.Name} {Settings.TitleAfterName}");
						OutlookService.SendEmail(subject, email, null, builder.ToString(), null, false, Settings.Signature);
					}
					activity.IsSendNotifications = false;
					Work.Activity.Update(activity);
				}

				Work.Complete();
			}
			catch (Exception ex)
			{
				_logger.ErrorAsync(ex.StackTrace);
				_logger.ErrorAsync(ex.Message);
			}
		}
		public void SendNotificationsToMe()
		{
			try
			{
				int rowindex = 1;
			string sheet = "Neohodnotene";
			string pathToExport = Path.Combine(Path.GetTempPath(), DateTime.Now.ToString("ddMMyyHHmmss", System.Globalization.CultureInfo.CurrentCulture) + "NeohodnoteneAktivity.xlsx");
			List<string> vs = new();
			MailAddressCollection mailAddresses = new();
			mailAddresses.Add(new MailAddress(Settings.Email));
			vs.Add(pathToExport);

		
				Excel.CreateSpreadsheetWorkbook(pathToExport, sheet);

				
				var act = Work.Activity.GetAll().Where(x => x.IsNotifyMe && DateTime.Today > x.Deadline);
				if (act == null || !act.Any())
				{
					return;
				}

				var grpId = act.FirstOrDefault().Student.MainGroup.Id;
				Settings = Work.Settings.GetSettingsByMainGroup(grpId);
				

				Excel.WriteRow(sheet, rowindex, true, "Skupina", "Predmet", "Student", "Aktivita", "Datum odovzdania");
				rowindex++;
				foreach (var activity in act.Where(x => x.Modified <= x.Created))
				{
					Excel.WriteRow(sheet, rowindex, true, activity.Student.MainGroup.Name, activity.Student.MainGroup.Subject, activity.Student.Name, activity.Name, $"{ activity.Deadline.ToShortDateString()} {activity.Deadline.ToShortTimeString()}");

					activity.IsNotifyMe = false;
					Work.Activity.Update(activity);
					rowindex++;
				}
				Excel.SaveFile();

				OutlookService.SendEmail("Aktivity na hodnotenie", mailAddresses, null, string.Empty, vs, false, Settings.Signature);

				Work.Complete();
			}
			catch (Exception ex)
			{
				_logger.ErrorAsync(ex.StackTrace);
				_logger.ErrorAsync(ex.Message);
			}
		}
	}
}
