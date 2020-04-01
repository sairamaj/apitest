using System;
using System.Collections.Generic;

namespace ApiManager.Model
{
	class Scenario
	{
		private List<Scenario> _children = new List<Scenario>();
		public Scenario(string name, string fileName, bool isContainer = false)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.FileName= fileName ?? throw new ArgumentNullException(nameof(fileName));
			this.IsContainer = isContainer;
			this._children = new List<Scenario>();
		}
		public string Name { get;  }
		public string FileName { get; }
		public bool IsContainer { get; }
		public IEnumerable<Scenario> Children { get { return this._children; } }
		public void AddScenario(Scenario scenario)
		{
			this._children.Add(scenario);
		}
	}
}
