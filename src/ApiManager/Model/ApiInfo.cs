using Newtonsoft.Json;
using System;

namespace ApiManager.Model
{
	public class ApiInfo : Info
	{
		private string _jwtToken;

		public ApiInfo()
		{
			this.Type = "Api";
		}

		public string Url { get; set; }
		public string Method { get; set; }
		public string StatusCode { get; set; }
		public long TimeTaken { get; set; }
		public Request Request { get; set; }
		public Response Response { get; set; }

		public string RelativeUrl
		{
			get
			{
				if (string.IsNullOrEmpty(this.Url))
				{
					return string.Empty;
				}

				return new Uri(this.Url).PathAndQuery;
			}
		}

		public string TimeTakenString
		{
			get
			{
				var timeTakenMilliseconds = (long)(((double)this.TimeTaken / 1000.0));
				return timeTakenMilliseconds.ToString();
			}
		}

		public string JwtToken
		{
			get
			{
				if (this._jwtToken == null)
				{
					this._jwtToken = this.ExtractJwtCode();
				}

				return this._jwtToken;
			}
		}

		private string ExtractJwtCode()
		{
			// Look in response first
			if (!string.IsNullOrWhiteSpace(this.Response.Content))
			{
				// try to extract
				try
				{
					var token = JsonConvert.DeserializeObject<JwtToken>(this.Response.Content);
					return token.Access_Token;
				}
				catch (Exception)
				{
					// Ignore deserialization of content if it is not JSON.
				}
			}

			return string.Empty;
		}
	}
}
