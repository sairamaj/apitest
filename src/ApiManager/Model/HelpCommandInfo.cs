using System.Collections.Generic;

namespace ApiManager.Model
{
	class HelpCommandInfo : Info
	{
		public IEnumerable<HelpCommand> Commands { get; set; }
	}
}
