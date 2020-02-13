using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiViewer.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiViewer.ViewModel
{
    internal class RequestViewModel : CoreViewModel
    {
        public RequestViewModel(Request request)
        {
            Request = request;
        }
        public Request Request { get; }

        public void Update()
        {
            OnPropertyChanged(()=> this.Request);
        }
    }
}
