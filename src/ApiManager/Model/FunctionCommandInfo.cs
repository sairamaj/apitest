using System.Collections.Generic;

namespace ApiManager.Model
{
	class FunctionCommandInfo : Info
	{
		public FunctionCommandInfo()
		{
			this.Functions = new List<FunctionCommand>();
		}

		public IEnumerable<FunctionCommand> Functions { get; set; }
	}
}
