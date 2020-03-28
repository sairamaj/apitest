using System.Collections.Generic;

namespace ApiManager.Model
{
	class ApiInfo
	{
		public ApiInfo(string name, string path)
		{
			this.Name = name;
			this.Path = path;
			this.Scenarios = new List<Scenario>();
			this.Environments = new List<Environment>();
		}

		public string Name { get; }
		public string Path { get; }
		public string Configuration { get; set; }
		public IEnumerable<Scenario> Scenarios { get; set; }
		public IEnumerable<Environment> Environments{ get; set; }
	}
}
