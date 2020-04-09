using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiManager.Pipes
{
	class PipeDataProcessor
	{
		IMessageListener _listener;
		Action<string> _onErrorAction;
		IDictionary<string, Action<string>> _commandActions = new Dictionary<string, Action<string>>();

		public PipeDataProcessor(IMessageListener listener, Action<string> onErrorAction)
		{
			this._listener = listener ?? throw new ArgumentNullException(nameof(listener));
			this._onErrorAction = onErrorAction ?? throw new ArgumentNullException(nameof(onErrorAction));
		}

		public void Add(string name, string command, Action<string> action)
		{
			_commandActions[command] = action;
			this._listener.SubScribe(name, msg =>
			{
				try
				{
					// the message contains : command|jsonMessage
					var parts = msg.Split('|');
					if (parts.Length < 2)
					{
						return;
					}

					var commandInfo = parts.First();
					if (this._commandActions.TryGetValue(commandInfo, out var commandAction))
					{
						commandAction(msg.Substring(commandInfo.Length + 1));
					}
					else
					{
						this._onErrorAction($"Unknown {commandInfo} ...");
					}
				}
				catch (Exception e)
				{
					this._onErrorAction(e.Message);
				}
			});
		}
	}
}
