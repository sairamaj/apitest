using ApiManager.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
    class InfoViewModel : CoreViewModel
    {
        public InfoViewModel(Info info)
        {
            Info = info;
        }

        public Info Info { get; }
    }
}
