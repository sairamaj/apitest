using System;
using System.Collections.Generic;
using System.IO;

namespace ApiManager.Model
{
	class Scenario
	{
		private List<Scenario> _children = new List<Scenario>();
		public Scenario(string fileName, bool isContainer = false)
		{
			this.FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
			this.Name = Path.GetFileNameWithoutExtension(fileName);
			this.IsContainer = isContainer;
			this._children = new List<Scenario>();
		}

		public string Name { get;  }
		public string FileName { get; }
		public bool IsContainer { get; }

		public string ContainerPath
		{
			get
			{
				return this.IsContainer ? this.FileName : Path.GetDirectoryName(this.FileName);
			}
		}

		public IEnumerable<Scenario> Children { get { return this._children; } }
		public void AddScenario(Scenario scenario)
		{
			this._children.Add(scenario);
		}
	}
}
