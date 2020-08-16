using System.Collections.Generic;

namespace ApiManager.Model
{
	class ApiRoute
	{
		public ApiRoute()
		{
			this.Headers = new Dictionary<string, string>();
		}

		public string Name { get; set; }
		public string Description { get; set; }
		public string Path { get; set; }
		public string BaseUrl { get; set; }
		public string[] BaseUrlVariables { get; set; }
		public IDictionary<string, string> Headers { get; set; }
		public string[] HeadersVariables { get; set; }
		public IDictionary<string, string> Body { get; set; }
		public string[] BodyVariables { get; set; }
		public bool IsDefault => this.Name == "_";
	}
}
