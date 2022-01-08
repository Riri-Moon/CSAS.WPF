using System.Diagnostics;
using System.Net.Mail;
using System.Runtime.InteropServices;
using O = Microsoft.Office.Interop.Outlook;
namespace CSAS.Services
{
	public class OutlookService
	{
		O.Application app = null;

		public OutlookService()
		{
			var outlookProcess = Process.GetProcessesByName("Outlook").FirstOrDefault();
			if (outlookProcess != null)
			{
				app = GetActiveObject("Outlook.Application") as O.Application;
			}
			else
			{
			}
		}

		public bool SendEmail(MailAddress sender, string subject, MailAddressCollection to, MailAddressCollection cc, string body, IList<string> attachments, bool modal)
		{
			// sender not in use currently - there should be exactly one account in Outlook
			bool result = false;

			O.Account account = null;
			foreach (O.Account a in app.Session.Accounts)
			{
				account = a;
				break;
			}		

			O.MailItem mailItem = app.CreateItem(O.OlItemType.olMailItem);

			mailItem.SendUsingAccount = account ?? throw new Exception($"Unable to find Outlook account");
			mailItem.Subject = subject;
			mailItem.To = string.Join(";", to.Select(t => t.Address));
			if (cc != null)
			{
				mailItem.CC = string.Join(";", cc.Select(c => c.Address));
			}
			//mailItem.Body = body;
			mailItem.HTMLBody = body;
			if (attachments?.Any() ?? false)
			{
				foreach (var attachment in attachments)
				{
					mailItem.Attachments.Add(attachment);
				}
			}

			if (modal)
			{
				mailItem.Display(true);

				try
				{
					var res = mailItem.Sent;
				}
				catch (Exception)
				{
					//email not found - moved to outbox/sent
					result = true;
				}
			}
			else
			{
				mailItem.Send();
				result = true;
			}

			return result;
		}

		public static object GetActiveObject(string progId, bool throwOnError = false)
		{
			if (progId == null)
				throw new ArgumentNullException(nameof(progId));

			var hr = CLSIDFromProgIDEx(progId, out var clsid);
			if (hr < 0)
			{
				if (throwOnError)
					Marshal.ThrowExceptionForHR(hr);

				return null;
			}

			hr = GetActiveObject(clsid, IntPtr.Zero, out var obj);
			if (hr < 0)
			{
				if (throwOnError)
					Marshal.ThrowExceptionForHR(hr);

				return null;
			}
			return obj;
		}

		[DllImport("ole32")]
		private static extern int CLSIDFromProgIDEx([MarshalAs(UnmanagedType.LPWStr)] string lpszProgID, out Guid lpclsid);

		[DllImport("oleaut32")]
		private static extern int GetActiveObject([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, IntPtr pvReserved, [MarshalAs(UnmanagedType.IUnknown)] out object ppunk);
	}
}
