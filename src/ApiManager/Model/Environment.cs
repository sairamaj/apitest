using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ApiManager.Model
{
	class Environment
	{
		public Environment(string name, string fileName)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
		}
		public string Name { get; }
		public string FileName { get; }

		public IDictionary<string, string> Variables
		{
			get
			{
				var variables = new Dictionary<string, string>();
				if (!File.Exists(this.FileName))
				{
					return variables;
				}

				foreach (var line in File.ReadAllLines(this.FileName))
				{
					var parts = line.Split('=');
					if (parts.Length > 1)
					{
						var key = parts.First();
						variables[key] = line.Substring(key.Length + 1);
					}
				}

				return variables;
			}
		}

	}
}
