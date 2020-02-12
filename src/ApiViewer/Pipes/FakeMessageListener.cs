using System;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ApiViewer.Model;

namespace ApiViewer.Pipes
{
    internal class FakeMessageListener : IMessageListener
    {
        public async Task SubScribe(Action<ApiInfo> onMessage)
        {
            await new TaskFactory().StartNew(() =>
            {
                do
                {
                    onMessage(new ApiInfo
                    {
                        Url = "http://localhost:3000/5001",
                        Method = "Get",
                        StatusCode = HttpStatusCode.Created,
                        TimeTakenInMilliseconds = 101,
                    });

                    Thread.Sleep(2000);
                } while (true);
            });
        }
    }
}
