using System;

namespace ApiManager.Pipes
{
	interface IApiTestConsoleCommunicator : IDisposable
	{
		void Add(string channel, string command, Action<string> onData);
	}
}
