using System.IO;
using System.Net.Mail;
using System.Text;

namespace CSAS.Helpers
{
	public class NotificationHelper
	{
		private UnitOfWork Work { get; set; }
		public Settings Settings { get; set; }
		public ExcelService excel { get; set; }
		private OutlookService OutlookService { get; set; }

		private readonly string space = "<br/><br/>";

		public NotificationHelper()
		{
			 excel = new();
			Work = new UnitOfWork(context: new AppDbContext());
			OutlookService = new OutlookService();

		}

		public void SendNotification()
		{
			StringBuilder builder = new();
			Settings = Work.Settings.GetAll().FirstOrDefault();
			var act = Work.Activity.GetAll().Where(x => x.IsSendNotifications);
			if(act==null || !act.Any())
			{
				return;
			}
			act = act.Where(x => (x.Deadline - DateTime.Today).TotalDays <= 3 );

			foreach (var activity in act.Where(x => x.Modified <= x.Created))
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
					builder.Append($"{space} S pozdravom {Settings.Title} {Settings.Name} {Settings.TitleAfterName}");
					OutlookService.SendEmail(new MailAddress(Settings.Email), subject, email, null, builder.ToString(), attachments, false);
				}
				else
				{
					builder.Append($"{space} S pozdravom {Settings.Title} {Settings.Name} {Settings.TitleAfterName}");
					OutlookService.SendEmail(new MailAddress(Settings.Email), subject, email, null, builder.ToString(), null, false);
				}
				activity.IsSendNotifications = false;
				Work.Activity.Update(activity);
			}

			Work.Complete();
		}
		public void SendNotificationsToMe()
		{	
			int rowindex = 1;
			string sheet = "Neohodnotene";
			string pathToExport = Path.Combine(Path.GetTempPath(), DateTime.Now.ToString("ddMMyyHHmmss", System.Globalization.CultureInfo.CurrentCulture) + "NeohodnoteneAktivity.xlsx");
			List<string> vs = new();
			MailAddressCollection mailAddresses = new();
			mailAddresses.Add(new MailAddress(Settings.Email));
			vs.Add(pathToExport);

			excel.CreateSpreadsheetWorkbook(pathToExport, sheet);

			Settings = Work.Settings.GetAll().FirstOrDefault();
			var act = Work.Activity.GetAll().Where(x => x.IsNotifyMe && DateTime.Today > x.Deadline);
			if (act == null || !act.Any())
			{
				return;
			}
			

			excel.WriteRow(sheet, rowindex, true, "Skupina", "Predmet", "Student", "Aktivita", "Datum odovzdania");
			rowindex++;
			foreach (var activity in act.Where(x=>x.Modified <= x.Created))
			{
				excel.WriteRow(sheet, rowindex, true, activity.Student.MainGroup.Name, activity.Student.MainGroup.Subject, activity.Student.Name, activity.Name, $"{ activity.Deadline.ToShortDateString()} {activity.Deadline.ToShortTimeString()}");

				activity.IsNotifyMe = false;
				Work.Activity.Update(activity);
				rowindex++;
			}
			excel.SaveFile();

			OutlookService.SendEmail(new MailAddress(Settings.Email), "Aktivity na hodnotenie", mailAddresses, null, string.Empty, vs, false);

			Work.Complete();
		}
	}
}
