using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiManager.Model
{
	class ManagementCommandInfo : Info
	{
		public IEnumerable<string> Commands { get; set; }
	}
}
