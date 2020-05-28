﻿using System.Collections.Generic;
using System.Windows.Input;
using ApiManager.ApiConfigEditing.ViewModels;
using ApiManager.ApiConfigEditing.Views;
using ApiManager.Model;
using ApiManager.Utils;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class CommandInfoViewModel : TreeViewItemViewModel
	{
		public CommandInfoViewModel(ApiCommand apiCommand, IEnumerable<string> subCommands)
			: base(null, apiCommand.Name, true)
		{
			ApiCommand = apiCommand;
			this.SubCommands = subCommands;
			this.IsExpanded = true;
			this.AddApiRouteCommand = new DelegateCommand(() =>
			{
				UiHelper.SafeAction(AddRoute, "Add Route");
			});
		}

		public ApiCommand ApiCommand { get; }
		public IEnumerable<string> SubCommands { get; }
		public ICommand AddApiRouteCommand { get; set; }

		protected override void LoadChildren()
		{
			foreach (var subCommand in this.SubCommands)
			{
				this.Children.Add(new SubCommandInfoViewModel(subCommand));
			}
		}

		private void AddRoute()
		{
			AddRouteWindow win = new AddRouteWindow();
			var vm = new AddRouteWindowViewModel(win);
			win.DataContext = vm;
			if (!win.ShowDialog().Value)
			{
				return;
			}
		}

	}
}
