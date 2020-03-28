using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiManager.Model
{
	class CommandInfo
	{
		public CommandInfo()
		{
			this.Commands = new List<string>();
		}

		public string SessionName { get; set; }
		public string ConfigFileName { get; set; }
		public string BatchFileName { get; set; }
		public IEnumerable<string> Commands { get; set; }
		public bool IsDebug { get; set; }
	}
}
