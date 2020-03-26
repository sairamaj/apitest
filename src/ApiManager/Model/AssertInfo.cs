namespace ApiManager.Model
{
	class AssertInfo : Info
    {
		public AssertInfo()
		{
			this.Type = "Assert";
		}

		public bool Success { get; set; }
		public string Message { get; set; }
		public string Variable { get; set; }
		public string Expected { get; set; }
		public string Actual { get; set; }
	}
}
