using System.Collections.Generic;

namespace ApiManager.Model
{
	class ManagementCommandInfo : Info
	{
		public ManagementCommandInfo()
		{
			Commands = new Dictionary<string, IEnumerable<string>>();
		}

		public IDictionary<string, IEnumerable<string>> Commands { get; set; }
	}
}
