using System;

namespace ApiManager.Pipes
{
	class ApiTestConsoleCommunicator : IApiTestConsoleCommunicator
	{
		private readonly IMessageListener _listener;
		private readonly PipeDataProcessor _processor;
		private bool _isDisposed;
		public ApiTestConsoleCommunicator(IMessageListener listener)
		{
			this._processor = new PipeDataProcessor(listener, (error) => { });
			this._listener = listener;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Add(string channel, string command, Action<string> onData)
		{
			this._processor.Add(channel, command, onData);
		}

		private void Dispose(bool isDisposing)
		{
			if (!isDisposing || _isDisposed)
			{
				return;
			}

			this._listener.Dispose();
			this._isDisposed = true;
		}
	}
}
