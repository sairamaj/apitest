using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class SubCommandInfoViewModel : TreeViewItemViewModel
	{
		public SubCommandInfoViewModel(string name) : base(null, name, true)
		{
			this.IsExpanded = true;
		}
	}
}
