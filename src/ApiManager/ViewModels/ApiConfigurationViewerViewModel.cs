using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApiManager.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class ApiConfigurationViewerViewModel : CoreViewModel
	{
		public ApiConfigurationViewerViewModel(ApiInfo apiInfo, ManagementCommandInfo commandInfo)
		{
			this.ApiInfo = apiInfo;
			this.ConfigurationData = File.ReadAllText(apiInfo.Configuration);
			this.Commands = commandInfo.Commands
				.OrderBy(c => c.Key)
				.Select(c =>new CommandInfoViewModel(c.Key, c.Value) );
		}

		public ApiInfo ApiInfo { get; }
		public string ConfigurationData { get; set; }
		public IEnumerable<CommandInfoViewModel> Commands { get; set; }
	}
}
