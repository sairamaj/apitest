using System;
using System.Collections.Generic;
using System.IO;
using ApiManager.Model;
using Newtonsoft.Json;

namespace ApiManager.Report
{
	internal class ReportGenerator
	{
		public static void Generate(IEnumerable<ApiRequest> requests, string folder)
		{
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			foreach (var request in requests)
			{
				var relativeUrl = request.RelativeUrl.Trim(new char[] { '/' });
				var requestFolder = Path.Combine(folder,
					$"{relativeUrl.Replace("/", "_")}-{request.Method}-{request.HttpCode}-{request.StatusCode}");
				if (!Directory.Exists(requestFolder))
				{
					Directory.CreateDirectory(requestFolder);
				}

				// Write url.txt			
				File.WriteAllText(Path.Combine(requestFolder, "url.txt"), request.Url.ToString());

				if (!string.IsNullOrWhiteSpace(request.Request.Body))
				{
					// Write request.json
					File.WriteAllText(Path.Combine(requestFolder, "request.json"), IndentIfJson(request.Request.Body));
				}

				if (request.Request.Headers != null)
				{
					// Write request.json
					File.WriteAllText(Path.Combine(requestFolder, "request_headers.json"), request.Request.HeadersAsString);
				}

				if (!string.IsNullOrWhiteSpace(request.Response.Content))
				{
					// Write response.json
					File.WriteAllText(Path.Combine(requestFolder, "response.json"), IndentIfJson(request.Response.Content));
				}

				if (request.Response.Headers != null)
				{
					// Write request.json
					File.WriteAllText(Path.Combine(requestFolder, "request_headers.json"), request.Response.HeadersAsString);
				}
			}
		}
		private static string IndentIfJson(string value)
		{
			try
			{
				return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(value), Formatting.Indented);
			}
			catch (Exception)
			{
			}

			return value;
		}

	}
}
