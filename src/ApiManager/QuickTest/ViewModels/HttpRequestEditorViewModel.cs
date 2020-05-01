using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Utils;
using Wpf.Util.Core.Command;

namespace ApiManager.QuickTest.ViewModels
{
	class HttpRequestEditorViewModel
	{
		public HttpRequestEditorViewModel(Environment environment)
		{
			this.HttpMethod = "GET";
			this.SendCommand = new DelegateCommand(
				() =>
				UiHelper.SafeAction(SendRequest, "Send"));
		}

		public string HttpMethod { get; set; }
		public string Url { get; set; }
		public string[] Methods
		{
			get
			{
				return new[] { "GET", "POST", "PUT", "DELETE" };
			}
		}

		public ICommand SendCommand { get; set; }

		private void SendRequest()
		{
			MessageBox.Show("sending...");
		}

	}
}