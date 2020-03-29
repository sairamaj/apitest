using System.Collections.Generic;

namespace ApiManager.Model
{
	class ManagementVariableInfo : Info
	{
		public IEnumerable<string> Variables { get; set; }
	}
}
