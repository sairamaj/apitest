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
			this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
			this.Route = api.Routes.First();
			this.Url = $"{Route.BaseUrl}/{Route.Path}";
			this.SubmitCommand = new DelegateCommand(async () => await this.Submit());
			this.HeaderItems = new HeaderItemsViewModel(this.Route.Headers);
			this.ApiRequest = new ApiRequest();
			this.ApiRequest.Request = new Request();
			this.ApiRequest.Request.Body = this.GetBody();
		}

		public ApiRoute Route { get; }
		public string Url { get; set; }
		public ApiCommand Api { get; }
		public ApiEnvironment Environment { get; }
		public HeaderItemsViewModel HeaderItems { get; }
		public ICommand SubmitCommand { get; }
		public string Response { get; set; }
		public ApiRequest ApiRequest { get; set; }

		private async Task Submit()
		{
			try
			{

				var apiRequest = this.ApiRequest;
				apiRequest.Method = "post";
				apiRequest.Request.Headers = this.HeaderItems.Items.ToDictionary(vm => vm.Name, vm => vm.Value);
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

		private string GetBody()
		{
			if (this.Route.Body == null)
			{
				return string.Empty;
			}

			var body = string.Empty;
			var envVariables = this.Environment.Variables;
			foreach (var kv in this.Route.Body) 
			{
				var val = kv.Value;
				if (val.StartsWith("{{") && val.EndsWith("}}"))
				{
					var variableName = kv.Value.Replace("{", string.Empty).Replace("}", string.Empty);
					if (envVariables.ContainsKey(variableName))
					{
						val = envVariables[variableName];
					}
				}
				body += $"{kv.Key}={val}&";
			}

			return body.Trim('&');
		}
	}
}
