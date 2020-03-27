using System.Collections.Generic;

namespace ApiManager.Model
{
	class ApiInfo
	{
		public ApiInfo(string name, string path)
		{
			this.Name = name;
			this.Path = path;
		}

		public string Name { get; }
		public string Path { get; }
		public string Configuration { get; set; }
		public IEnumerable<Scenario> Scenarios { get; set; }
		public IEnumerable<string> VariableFiles { get; set; }
	}
}
