using ApiViewer.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiViewer.ViewModel
{
    internal class ResponseViewModel : CoreViewModel
    {
        public ResponseViewModel(Response response)
        {
            Response = response;
        }
        public Response Response { get; }
        public void Update()
        {
            OnPropertyChanged(() => this.Response);
        }
    }
}
