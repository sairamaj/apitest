namespace ApiManager.Model
{
	class ExtractVariableInfo : Info
    {
		public ExtractVariableInfo()
		{
			this.Type = "Extract";
		}

		public string Variable { get; set; }
		public string Value { get; set; }
	}
}
