using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			this.Commands = commandInfo.ApiCommands.Select(c => new CommandInfoViewModel(c.Name, c.Routes.Select(r => r.Name)));
		}

		public ApiInfo ApiInfo { get; }
		public string ConfigurationData { get; set; }
		public IEnumerable<CommandInfoViewModel> Commands { get; set; }
	}
}
