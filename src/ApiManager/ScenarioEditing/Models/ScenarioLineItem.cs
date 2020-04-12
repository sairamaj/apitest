namespace ApiManager.ScenarioEditing.Models
{
	class ScenarioLineItem
	{
		public ScenarioLineItem(string type)
		{
			Type = type;
		}

		public string Type { get; set; }
	}
}
