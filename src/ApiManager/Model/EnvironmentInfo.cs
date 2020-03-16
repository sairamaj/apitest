using System.Collections.Generic;

namespace ApiManager.Model
{
	class EnvironmentInfo
	{
		public EnvironmentInfo(string name)
		{
			this.Name = name;
		}

		public string Name { get; }
		public string Configuration { get; }
		public IEnumerable<string> CommandFiles { get; set; }
		public IEnumerable<string> VariableFiles { get; set; }
	}
}
