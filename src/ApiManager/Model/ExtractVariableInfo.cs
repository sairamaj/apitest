namespace ApiManager.Model
{
	class ExtractVariableInfo : Info
    {
		public ExtractVariableInfo()
		{
			this.Type = "Extract";
		}

		public bool Success { get; set; }
		public string Message { get; set; }
		public string Variable { get; set; }
		public string Value { get; set; }
		public string ShortValue
		{
			get
			{
				if (this.Value == null)
				{
					return string.Empty;
				}

				var val = this.Value.Substring(0, this.Value.Length > 50 ? 50 : this.Value.Length);
				if (this.Value.Length > 50)
				{
					val += "......";
				}

				return val;
			}
		}

		public string ValueOrMessage
		{
			get
			{
				if (this.Success)
				{
					return this.Value;
				}

				return this.Message;
			}
			set
			{
			}
		}
	}
}
