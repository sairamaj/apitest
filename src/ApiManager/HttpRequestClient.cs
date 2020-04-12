using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiManager.Model;
using ApiManager.Pipes;
using ApiManager.Repository;
using Newtonsoft.Json;

namespace ApiManager
{
	internal class HttpRequestClient
	{
		public HttpRequestClient(ApiRequest request)
		{
			this.Request = request;
		}

		public async Task<ApiRequest> GetResponseAsync()
		{
			var requestFileName = CreateRequestFile();
			ApiRequest newApiRequest = null;
			using (var communicator = ServiceLocator.Locator.Resolve<IApiTestConsoleCommunicator>())
			{
				communicator.Add("apiinfo", "api", data =>
				{
					newApiRequest = JsonConvert.DeserializeObject<ApiRequest>(data);
				});

				await ServiceLocator.Locator.Resolve<ICommandExecutor>()
					.SubmitHttpRequest(
					requestFileName, 
					Guid.NewGuid().ToString())
					.ConfigureAwait(false);
			}

			return await Task.FromResult(newApiRequest).ConfigureAwait(false);
		}

		public ApiRequest Request { get; }

		private string CreateRequestFile()
		{
			var requestBuilder = new StringBuilder();
			requestBuilder.AppendLine(this.Request.Method);
			requestBuilder.AppendLine(this.Request.Url);
			requestBuilder.AppendLine("");
			requestBuilder.AppendLine("headers_begin");
			foreach (var header in this.Request.Request.Headers)
			{
				requestBuilder.AppendLine($"{header.Key}:{header.Value}");
			}
			requestBuilder.AppendLine("headers_end");

			requestBuilder.AppendLine("");
			requestBuilder.AppendLine("request_begin");
			requestBuilder.AppendLine(this.Request.Request.Body);
			requestBuilder.AppendLine("request_end");

			return FileHelper.WriteToTempFile(requestBuilder.ToString(), ".txt");
		}
	}
}
