using ApiManager.Model;
using ApiManager.Repository;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
    class InfoViewModel : CoreViewModel
    {
        public InfoViewModel(ICommandExecutor executor, Info info)
        {
			Executor = executor;
			Info = info;
        }

		public ICommandExecutor Executor { get; }
		public Info Info { get; }
    }
}
