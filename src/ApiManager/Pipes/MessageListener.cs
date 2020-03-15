using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ApiManager.Model;
using Newtonsoft.Json;

namespace ApiManager.Pipes
{
	internal class MessageListener : IMessageListener
	{
		public async Task SubScribe(Action<ApiInfo> onMessage)
		{

			do
			{
				var pipeClient =
					new NamedPipeClientStream(
						".",
						"Foo",
						PipeDirection.In, PipeOptions.None,
						TokenImpersonationLevel.Impersonation);

				TraceLogger.Debug("MessageListener.SubScribe.Connect");
				await pipeClient.ConnectAsync();
				TraceLogger.Debug("MessageListener.SubScribe.Connected");
				try
				{
					do
					{
						var data = await new StreamString(pipeClient).ReadStringAsync();
						try
						{
							onMessage(JsonConvert.DeserializeObject<ApiInfo>(data));
						}
						catch (Exception e)
						{
							TraceLogger.Error($"MessageListener.SubScribe.Deserialize:{e}");
						}
					} while (true);
				}
				catch (Exception e)
				{
					TraceLogger.Error($"MessageListener.SubScribe.Read:{e}");
				}
			} while (true);
		}

		class StreamString
		{
			private readonly Stream _isStream;

			public StreamString(Stream isStream)
			{
				_isStream = isStream;
			}

			public async Task<string> ReadStringAsync()
			{
				var header = new byte[4];
				await _isStream.ReadAsync(header, 0, header.Length);
				var len = BitConverter.ToUInt32(header, 0);
				var bytes = new byte[len];
				await _isStream.ReadAsync(bytes, 0, (int)len);
				return Encoding.UTF8.GetString(bytes);
			}
		}
	}
}
