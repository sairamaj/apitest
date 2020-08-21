using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using ApiManager.Model;
using Newtonsoft.Json;
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

			var translatedHeaders = new Dictionary<string, string>();
			foreach (var header in this.Route.Headers)
			{
				translatedHeaders[header.Key] = Evaluator.TranslateHeaderItem(header.Value,
					environment.Variables);
			}
			this.HeaderItems = new HeaderItemsViewModel(translatedHeaders);
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
		public bool IsSuccess { get; set; }
		public string AccessToken { get; set; }

		private async Task Submit()
		{
			try
			{

				this.ApiRequest.Method = "post";
				this.ApiRequest.Request.Headers = this.HeaderItems.Items.ToDictionary(vm => vm.Name, vm => vm.Value);
				this.ApiRequest.Url = this.Url;

				var request = new HttpRequestClient(this.ApiRequest);
				this.ApiRequest = await request.GetResponseAsync().ConfigureAwait(false);
				this.Response = this.ApiRequest?.Response?.Content;
				
				this.IsSuccess = this.ApiRequest.HttpCode == (int)HttpStatusCode.OK;
				if (this.IsSuccess)
				{
					this.AccessToken = ExtractAccessToken(this.ApiRequest.Response.Content);
				}

				OnPropertyChanged(() => this.Response);
				OnPropertyChanged(() => this.ApiRequest);
				OnPropertyChanged(() => this.IsSuccess);

			}
			catch (Exception e)
			{
				this.Response = e.ToString();
				OnPropertyChanged(() => this.Response);
			}
		}

		private string ExtractAccessToken(string response)
		{
			return JsonConvert.DeserializeObject<JwtToken>(response).Access_Token;
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
