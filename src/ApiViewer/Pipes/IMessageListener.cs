using System;
using System.Threading.Tasks;

namespace ApiViewer.Pipes
{
    internal interface IMessageListener
    {
        Task SubScribe(Action<string> onMessage);
    }
}
