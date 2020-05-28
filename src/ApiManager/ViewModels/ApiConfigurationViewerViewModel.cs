using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApiManager.ApiConfigEditing.ViewModels;
using ApiManager.ApiConfigEditing.Views;
using ApiManager.Model;
using ApiManager.Utils;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class ApiConfigurationViewerViewModel : CoreViewModel
	{
		public ApiConfigurationViewerViewModel(ApiInfo apiInfo, ApiCommandInfo commandInfo)
		{
			this.ApiInfo = apiInfo;
			this.ConfigurationData = File.ReadAllText(apiInfo.Configuration);
			this.Commands = commandInfo.ApiCommands.Select(c => new CommandInfoViewModel(c, c.Routes.Select(r => r.Name)));
		}

		public ApiInfo ApiInfo { get; }
		public string ConfigurationData { get; set; }
		public IEnumerable<CommandInfoViewModel> Commands { get; set; }
	}
}
