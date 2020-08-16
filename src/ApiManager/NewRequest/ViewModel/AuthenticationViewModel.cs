using System.Linq;
using ApiManager.Model;
using Wpf.Util.Core.ViewModels;
using ApiEnvironment = ApiManager.Model.Environment;

namespace ApiManager.NewRequest.ViewModel
{
	internal class AuthenticationViewModel : CoreViewModel
	{
		public AuthenticationViewModel(ApiCommand api, ApiEnvironment environment)
		{
			this.Api = api;
			var route = api.Routes.First();
			this.Url = $"{this.Api.Name}:{route.BaseUrl}/{route.Path}";
		}

		public ApiRoute Route { get; }
		public string Url { get; set; }
		public ApiCommand Api { get; }
	}
}
