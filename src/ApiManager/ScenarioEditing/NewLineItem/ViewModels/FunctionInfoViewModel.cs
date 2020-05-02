using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class FunctionInfoViewModel : CommandTreeViewModel
	{
		public FunctionInfoViewModel( string name, string tag) : base(null, name, tag)
		{
			this.IsExpanded = true;
		}
	}
}
