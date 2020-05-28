using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using ApiManager.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class ApiConfigurationViewerViewModel : CoreViewModel
	{
		public ApiConfigurationViewerViewModel(ApiInfo apiInfo, ApiCommandInfo commandInfo)
		{
			this.ApiInfo = apiInfo;
			this.ConfigurationData = File.ReadAllText(apiInfo.Configuration);
			this.Commands = commandInfo.ApiCommands.Select(c => new CommandInfoViewModel(
				c,
				c.Routes.Select(r => r.Name),
				apiRoute => this.Save(apiInfo.Configuration)));
		}

		public ApiInfo ApiInfo { get; }
		public string ConfigurationData { get; set; }
		public IEnumerable<CommandInfoViewModel> Commands { get; set; }
		private void Save(string configuration)
		{
			MessageBox.Show($"saving {configuration}");
		}

	}
}
