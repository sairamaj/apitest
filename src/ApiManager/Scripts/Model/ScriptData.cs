using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiManager.Scripts.Models
{
	class ScriptData
	{
		public ScriptData(string name, string fileName)
		{
			Name = name;
			FileName = fileName;
		}

		public string Name { get; }
		public string FileName { get; }
	}
}
