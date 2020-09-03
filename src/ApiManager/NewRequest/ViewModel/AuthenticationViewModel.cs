using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Repository;
using Newtonsoft.Json;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;
using ApiEnvironment = ApiManager.Model.Environment;

namespace ApiManager.NewRequest.ViewModel
{
	internal class AuthenticationViewModel : CoreViewModel
	{
		private readonly ICacheManager _cacheManager;

		public AuthenticationViewModel(ICacheManager cacheManager, ApiCommand api, ApiEnvironment environment)
		{
			this._cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
			this.Api = api;
			this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
			this.Route = api.Routes.First();
			this.Url = Evaluator.Evaluate($"{Route.BaseUrl}/{Route.Path}", environment.Variables);
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

			this.OriginalApiRequest = new ApiRequest();
			this.OriginalApiRequest.Request = new Request();
			this.OriginalApiRequest.Request.Body = this.GetBody();

			this.RequestVariables = new VariableEditViewModel(Evaluator.GetVariables(this.OriginalApiRequest.Request.Body).ToDictionary(v => v, v => string.Empty), (name, value)=>
			{
				if (ApiRequest.Request == null)
				{
					return;
				}

				ApiRequest.Request.Body = Evaluator.Evaluate(OriginalApiRequest.Request.Body, this.RequestVariables.VariableWithValues);
				OnPropertyChanged(() => this.ApiRequest);
			});
		}

		public ApiRoute Route { get; }
		public string Url { get; set; }
		public ApiCommand Api { get; }
		public ApiEnvironment Environment { get; }
		public HeaderItemsViewModel HeaderItems { get; }
		public ICommand SubmitCommand { get; }
		public string Response { get; set; }
		public ApiRequest ApiRequest { get; set; }
		public ApiRequest OriginalApiRequest { get; set; }
		public bool IsSuccess { get; set; }
		public string AccessToken { get; set; }

		public VariableEditViewModel RequestVariables { get; }
		
		private async Task Submit()
		{
			try
			{
				var newApiRequest = new ApiRequest();
				newApiRequest.Method = "post";
				newApiRequest.Request = this.ApiRequest.Request;
				newApiRequest.Request.Url = this.Url;
				newApiRequest.Request.Headers = this.HeaderItems.Items.ToDictionary(vm => vm.Name, vm => vm.Value);
				newApiRequest.Url = this.Url;

				var request = new HttpRequestClient(newApiRequest);
				newApiRequest = await request.GetResponseAsync().ConfigureAwait(false);
				if (newApiRequest != null)
				{
					this.Response = newApiRequest.Response?.Content;
					this.IsSuccess = newApiRequest.HttpCode == (int)HttpStatusCode.OK;
					if (this.IsSuccess)
					{
						this.AccessToken = ExtractAccessToken(newApiRequest.Response.Content);
						if (!string.IsNullOrEmpty(this.AccessToken))
						{
							this._cacheManager.Add("AccessToken", this.AccessToken);
						}
					}
					this.ApiRequest = newApiRequest;
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
				var val = Evaluator.Evaluate(kv.Value, envVariables);
				body += $"{kv.Key}={val}&";
			}

			return body.Trim('&');
		}
	}
}
