using System;
using System.Threading.Tasks;
using ApiManager.Model;

namespace ApiManager.Pipes
{
	internal interface IMessageListener : IDisposable
	{
		Task SubScribe(string name, Action<string> onMessage);
	}
}
