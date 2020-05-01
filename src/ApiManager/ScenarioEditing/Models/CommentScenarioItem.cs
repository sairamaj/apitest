﻿namespace ApiManager.ScenarioEditing.Models
{
	class CommentScenarioItem : ScenarioLineItem
	{
		public CommentScenarioItem(string line) : base("comment", line)
		{
			this.Line = line;
		}

		public string Line { get; }
	}
}
