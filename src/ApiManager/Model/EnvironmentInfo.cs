﻿using System.Collections.Generic;

namespace ApiManager.Model
{
	class EnvironmentInfo
	{
		public EnvironmentInfo(string name, string path)
		{
			this.Name = name;
			this.Path = path;
		}

		public string Name { get; }
		public string Path { get; }
		public string Configuration { get; }
		public IEnumerable<string> CommandFiles { get; set; }
		public IEnumerable<string> VariableFiles { get; set; }
	}
}