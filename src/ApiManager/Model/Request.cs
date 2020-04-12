using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiManager.Model
{
	public class Request
	{
		public string Url { get; set; }
		public string Body { get; set; }
		public IDictionary<string, string> Headers { get; set; }
		public string HeadersAsString
		{
			get
			{
				if (this.Headers == null)
				{
					return string.Empty;
				}

				return JsonConvert.SerializeObject(this.Headers, Formatting.Indented);
			}
			set
			{
			}
		}

		public string GetHeaderValue(string name)
		{
			if (this.Headers == null)
			{
				return null;
			}

			if (this.Headers.TryGetValue(name, out var val))
			{
				return val;
			}

			return null;
		}
		public string RequestData
		{
			get => $"Url:{this.Url}\r\n\r\nHeaders:\r\n{this.HeadersAsString}\r\nBody:\r\n{this.Body}";
			set
			{

			}
		}

		public string RequestWithoutBody
		{
			get => $"Url:{this.Url}\r\n\r\nHeaders:\r\n{this.HeadersAsString}";
			set
			{
			}
		}
	}
}
