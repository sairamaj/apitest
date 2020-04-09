using System;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApiManager.Model;
using Newtonsoft.Json;

namespace ApiManager.Pipes
{
	internal class FakeMessageListener : IMessageListener
	{
		public async Task SubScribe(Action<ApiRequest> onMessage)
		{
			await new TaskFactory().StartNew(() =>
			{
				do
				{
					onMessage(new ApiRequest
					{
						Url = "http://localhost:3000/5001",
						Method = "Get",
						StatusCode = HttpStatusCode.Created.ToString(),
						Request = new Request
						{
							Body = "This is body, large body This is body, large bodyThis is body, large bodyThis is body, large bodyThis is body, large bodyThis is body, large bodyThis is body, large bodyThis is body, large bodyThis is body, large bodyThis is body, large bodyThis is body, large bodyThis is body, large bodyThis is body, large body"
						},
						Response = new Response
						{
							Content = "{ \"message\": \"this is message\"}"
						}
					});

					Thread.Sleep(2000);
				} while (true);
			});
		}

		public async Task SubScribe(string name, Action<string> onMessage)
		{
			await Task.Delay(0);
		}

		public Task UnSubscribeAll()
		{
			throw new NotImplementedException();
		}
	}
}
