﻿using System.Collections.Generic;
using System.Linq;
using ApiManager.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class BangContainerCommandInfoViewModel : CommandTreeViewModel
	{
		public BangContainerCommandInfoViewModel(IEnumerable<BangCommandInfo> bangCommands) : base(null, "bang_commands","bang_commands")
		{
			this.BangCommandInfos = bangCommands.Select(b => new BangCommandInfoViewModel(b));
			this.IsExpanded = true;
		}

		public IEnumerable<BangCommandInfoViewModel> BangCommandInfos { get; }

		protected override void LoadChildren()
		{
			foreach (var info in BangCommandInfos)
			{
				this.Children.Add(info);
			}
		}
	}
}
