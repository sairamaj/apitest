using System.Collections.Generic;

namespace ApiManager.Model
{
	class ApiCommandInfo : Info
	{
		public ApiCommandInfo()
		{
			ApiCommands = new List<ApiCommand>();
		}

		public IEnumerable<ApiCommand> ApiCommands { get; set; }
	}
}
