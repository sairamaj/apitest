using System;

namespace ApiManager.Model
{
	class Scenario
	{
		public Scenario(string name, string fileName)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.FileName= fileName ?? throw new ArgumentNullException(nameof(fileName));
		}
		public string Name { get;  }
		public string FileName { get; }
	}
}
