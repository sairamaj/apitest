using System.Collections.Generic;
using System.Linq;
using ApiManager.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class FunctionContainerCommandInfoViewModel : CommandTreeViewModel
	{
		public FunctionContainerCommandInfoViewModel(FunctionCommandInfo functionCommandInfo) : base(null, "functions","functions")
		{
			this.FunctionCommandInfos = functionCommandInfo.Functions
				.OrderBy(b => b.Name )
				.Select(b => new FunctionCommandInfoViewModel(b));
			this.IsExpanded = true;
		}

		public IEnumerable<FunctionCommandInfoViewModel> FunctionCommandInfos { get; }

		protected override void LoadChildren()
		{
			foreach (var info in FunctionCommandInfos)
			{
				this.Children.Add(info);
			}
		}
	}
}
