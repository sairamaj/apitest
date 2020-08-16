using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ApiManager.Model;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;
using ApiEnvironment = ApiManager.Model.Environment;

namespace ApiManager.NewRequest.ViewModel
{
	internal class AuthenticationViewModel : CoreViewModel
	{
		public AuthenticationViewModel(ApiCommand api, ApiEnvironment environment)
		{
			this.Api = api;
			this.Route = api.Routes.First();
			this.Url = $"{Route.BaseUrl}/{Route.Path}";
			this.SubmitCommand = new DelegateCommand(async () => await this.Submit());
		}

		public ApiRoute Route { get; }
		public string Url { get; set; }
		public ApiCommand Api { get; }
		public ICommand SubmitCommand { get; }
		public string Response { get; set; }
		public ApiRequest ApiRequest { get; set; }

		private async Task Submit()
		{
			try
			{

				var apiRequest = new ApiRequest();
				apiRequest.Method = "post";
				apiRequest.Request = new Request();
				apiRequest.Request.Headers = this.Route.Headers;
				apiRequest.Request.Body = "grant_type=password&username=sairamaj%40hotmail.com&password=ssSS1234~~~";
				apiRequest.Url = this.Url;

				var request = new HttpRequestClient(apiRequest);
				this.ApiRequest = await request.GetResponseAsync().ConfigureAwait(false);
				this.Response = this.ApiRequest?.Response?.Content;
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
