using System.Collections.Generic;

namespace ApiManager.Model
{
	class BangCommandInfo : Info
	{
		public BangCommandInfo()
		{
			this.BangCommands = new List<BangCommand>();
		}

		public IEnumerable<BangCommand> BangCommands { get; set; }
	}
}
