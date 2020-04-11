using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiManager.Pipes
{
	internal class MessageListener : IMessageListener, IDisposable
	{
		CancellationTokenSource _source;
		private bool _isDisposed;

		public MessageListener()
		{
			_source = new CancellationTokenSource();
		}
		public async Task SubScribe(string name, Action<string> onMessage)
		{
			if (this._isDisposed)
			{
				throw new ObjectDisposedException("MessageListner has been disposed already.");
			}

			var quit = false;
			var pipeClient =
				new NamedPipeClientStream(
					".",
					name,
					PipeDirection.In, PipeOptions.None,
					TokenImpersonationLevel.Impersonation);
			do
			{
				try
				{
					TraceLogger.Debug("MessageListener.SubScribe.Connect");
					await pipeClient.ConnectAsync(_source.Token);
					TraceLogger.Debug("MessageListener.SubScribe.Connected");
					do
					{
						var data = await new StreamString(pipeClient).ReadStringAsync();
						try
						{
							onMessage(data);
						}
						catch (Exception e)
						{
							TraceLogger.Error($"MessageListener.SubScribe.Deserialize:{e}");
						}
					} while (true);
				}
				catch (OperationCanceledException)
				{
					TraceLogger.Debug($"MessageListener.SubScribe OperationCanceledException. quitting");
					quit = true;
				}
				catch (Exception e)
				{
					TraceLogger.Error($"MessageListener.SubScribe.Read:{e}");
				}
			} while (!quit);

			TraceLogger.Debug("MessageListener.SubScribe closing the pipe.");
			pipeClient.Close();
		}

		public async Task UnSubscribeAll()
		{
			TraceLogger.Debug("MessageListener.UnSubscribeAll cancelling.");
			this._source.Cancel();
			TraceLogger.Debug("MessageListener.UnSubscribeAll cancelled.");
			await Task.Delay(0).ConfigureAwait(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing)
		{
			if (!isDisposing || _isDisposed)
			{
				return;
			}

			this.UnSubscribeAll().Wait();
			this._isDisposed = true;
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
