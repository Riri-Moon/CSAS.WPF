using IBM.Tools.Common.Helper.Logger;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading;
using O = Microsoft.Office.Interop.Outlook;
namespace CSAS.Services
{
	public class OutlookService
	{
		readonly Logger _logger = new();
		readonly O.Application app = null;
		public OutlookService()
		{
			var outlookProcess = Process.GetProcessesByName("Outlook").FirstOrDefault();
			if (outlookProcess != null)
			{
				app = GetActiveObject("Outlook.Application") as O.Application;
			}
			else
			{
				var startInfo = new ProcessStartInfo()
				{
					FileName = "Outlook.exe",
					UseShellExecute = true
				};
				Process.Start(startInfo);

				while (app == null)
				{
					Thread.Sleep(1000);
					app = GetActiveObject("Outlook.Application") as O.Application;

				}

			}
		}

		public bool SendEmail(string subject, MailAddressCollection to, MailAddressCollection cc, string body, IList<string> attachments, bool modal, string signaturePath = "")
		{
			bool result = false;

			try
			{
				try
				{
					signaturePath = File.ReadAllText(signaturePath);
				}
				catch (Exception ex)
				{
					_logger.InfoAsync("Path to signature was empty");
				}

				O.MailItem mailItem = app.CreateItem(O.OlItemType.olMailItem);
				const string PR_SECURITY_FLAGS = "http://schemas.microsoft.com/mapi/proptag/0x6E010003";
				var ulFlags = 0x0;
				ulFlags |= 0x0; //  SECFLAG_ENCRYPTED
								//ulFlags = (ulFlags | 0x2); //  SECFLAG_SIGNED
				mailItem.PropertyAccessor.SetProperty(PR_SECURITY_FLAGS, ulFlags);
				mailItem.Subject = subject;
				mailItem.To = string.Join(";", to.Select(t => t.Address));
				if (cc != null)
				{
					mailItem.CC = string.Join(";", cc.Select(c => c.Address));
				}
				mailItem.HTMLBody = body + "<br></br>" + signaturePath;
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
			}
			catch (Exception ex)
			{
				_logger.ErrorAsync($"Outlook Service - {ex.Message}");
			}

			return result;
		}
		public IEnumerable<string> GetEmailAddresses()
		{
			foreach (O.Account a in app.Session.Accounts)
			{
				yield return a.SmtpAddress;
				//a.CurrentUser.AddressEntry.Address;
				//a.CurrentUser.AddressEntry.GetExchangeUser()?.PrimarySmtpAddress;
			}
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
