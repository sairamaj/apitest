using System.Collections.Generic;

namespace ApiManager.Model
{
	class ApiCommandInfo : Info
	{
		public ApiCommandInfo()
		{
			ApiCommands = new Dictionary<string, IEnumerable<string>>();
		}

		public IDictionary<string, IEnumerable<string>> ApiCommands { get; set; }
	}
}
