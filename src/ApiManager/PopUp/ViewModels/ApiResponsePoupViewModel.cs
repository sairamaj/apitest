using System;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.PopUp.ViewModels
{
	internal class ApiResponsePoupViewModel : CoreViewModel
	{
		public ApiResponsePoupViewModel(string url, string method, string request, string response)
		{
			this.Method = method;
			this.Request = request;
			this.Response = response;
			this.Path = new Uri(url).AbsolutePath;
		}

		public string Method { get; }
		public string Request { get; set;  }
		public string Response { get; set; }
		public string Path { get; }
		public string Title => $"{this.Method} {this.Path}";
	}
}
