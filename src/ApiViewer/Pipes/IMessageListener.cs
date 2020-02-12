using System;
using System.Threading.Tasks;
using ApiViewer.Model;

namespace ApiViewer.Pipes
{
    internal interface IMessageListener
    {
        Task SubScribe(Action<ApiInfo> onMessage);
    }
}
