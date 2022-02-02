using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSAS.Views.Controls;
namespace CSAS.Helpers
{
	public class MessageBoxHelper
	{
		public static bool? Show(string title, string text, bool isOkBtn)
		{
			YesNoMessageBoxWindow msg = new(isOkBtn)
			{
				TitleBar = { Text = title },
				Textbar = { Text = text },
			};
			msg.noBtn.Focus();
			return msg.ShowDialog();
		}
	}
}
