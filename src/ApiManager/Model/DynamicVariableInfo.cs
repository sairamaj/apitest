using System.Collections.Generic;

namespace ApiManager.Model
{
	class DynamicVariableInfo : Info
	{
		public DynamicVariableInfo()
		{
			this.DynamicVariables = new List<DynamicVariable>();
		}

		public IEnumerable<DynamicVariable> DynamicVariables { get; set; }
	}
}
