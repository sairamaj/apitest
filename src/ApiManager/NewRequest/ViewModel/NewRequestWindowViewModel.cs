using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.NewRequest.ViewModel
{
	class NewRequestWindowViewModel : CoreViewModel
	{
		public NewRequestWindowViewModel()
		{
			this.SelectedMethod = HttpMethods.First();
			this.Url = "https://api.enterprise.apigee.com/v1/organizations/sairamaj-eval/apis";
			this.SubmitCommand = new DelegateCommand(async() => await Submit().ConfigureAwait(false));
		}

		public string[] HttpMethods => new string[]{ "GET","POST","PUT","PATCH","DELETE"};
		public string SelectedMethod { get; set; }
		public string Url { get; set; }
		public ICommand SubmitCommand { get; }
		public string Response { get; set; }
		public ApiRequest ApiRequest { get; set; }
		private async Task Submit()
		{
			try
			{

				var apiRequest = new ApiRequest();
				apiRequest.Method = this.SelectedMethod;
				apiRequest.Request = new Request();
				apiRequest.Request.Headers = new Dictionary<string, string>();
				apiRequest.Url = this.Url;

				var request = new HttpRequestClient(apiRequest);
				this.ApiRequest = await request.GetResponseAsync().ConfigureAwait(false);
				this.Response = this.ApiRequest.Response.Content;
				OnPropertyChanged(() => this.Response);
				OnPropertyChanged(() => this.ApiRequest);
			}
			catch (Exception e)
			{
				this.Response = e.ToString();
				OnPropertyChanged(() => this.Response);
			}
		}
	}
}
