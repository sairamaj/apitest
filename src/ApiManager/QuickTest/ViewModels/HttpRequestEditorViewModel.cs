using ApiManager.Model;

namespace ApiManager.QuickTest.ViewModels
{
	class HttpRequestEditorViewModel
	{
		public HttpRequestEditorViewModel(Environment environment)
		{
			this.HttpMethod = "GET";
		}

		public string HttpMethod { get; set; }
		public string[] Methods
		{
			get
			{
				return new[] { "GET", "POST", "PUT", "DELETE" };
			}
		}
	}
}