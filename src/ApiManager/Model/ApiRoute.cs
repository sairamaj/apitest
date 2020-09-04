using System.Collections.Generic;

namespace ApiManager.Model
{
	class ApiRoute
	{
		public ApiRoute()
		{
			this.Headers = new Dictionary<string, string>();
		}

		public string ApiName { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Path { get; set; }
		public string BaseUrl { get; set; }
		public string FullUrl => $"{this.BaseUrl}{this.Path}";
		public string[] BaseUrlVariables { get; set; }
		public IDictionary<string, string> Headers { get; set; }
		public string[] HeadersVariables { get; set; }
		public IDictionary<string, string> Body { get; set; }
		public string[] BodyVariables { get; set; }
		public bool IsDefault => this.Name == "_";
		public string FullName
		{
			get
			{
				if (this.IsDefault)
				{
					return this.ApiName;
				}

				return $"{this.ApiName}.{this.Name}";
			}
		}

		public string RequestBodyAsString
		{
			get
			{
				if (this.Body == null)
				{
					return string.Empty;
				}

				var bodyString = string.Empty;
				foreach (var kv in this.Body)
				{
					bodyString += $"{kv.Key}={kv.Value}&";
				}

				return bodyString.Trim('&');
			}
		}
	}
}
