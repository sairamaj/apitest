using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiManager.ScenarioEditing.Models;

namespace ApiManager.ScenarioEditing
{
	class ScenarioParser
	{
		private readonly bool _partial;

		public ScenarioParser()
		{
		}

		public ScenarioParser(bool partial)
		{
			this._partial = partial;
		}

		public ScenarioLineItem Parse(string line)
		{
			line = line.Trim();
			if (line.StartsWith("#", System.StringComparison.OrdinalIgnoreCase))
			{
				if (line.Length > 1 && !this._partial)
				{
					// give one more try to see whether it is commented command or not or real comment
					var lineItem = new ScenarioParser(true).Parse(line.Substring(1).Trim());
					lineItem.IsCommented = true;
					return lineItem;
				}

				return new CommentScenarioItem(line);
			}
			else if (line.Trim().Length == 0)
			{
				return new LineBreakScenarioItem();
			}
			else if (line.StartsWith("!", System.StringComparison.OrdinalIgnoreCase))
			{
				return new CommandScenarioItem(line);
			}
			else if (line.StartsWith("__", System.StringComparison.OrdinalIgnoreCase))
			{
				return new FunctionScenarioItem(line);
			}
			else
			{
				return new ApiScenarioItem(line);
			}
		}
	}
}
