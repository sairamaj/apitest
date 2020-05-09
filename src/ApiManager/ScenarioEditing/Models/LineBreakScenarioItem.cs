using System;

namespace ApiManager.ScenarioEditing.Models
{
	class LineBreakScenarioItem : ScenarioLineItem
	{
		public LineBreakScenarioItem() 
			: base("linebreak","")
		{
		}

		public override string GetCommand()
		{
			return string.Empty;
		}
	}
}
