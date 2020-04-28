namespace ApiManager.Model
{
	class PrintInfo : Info
    {
		public PrintInfo()
		{
			this.Type = "Print";
		}

		public string Message { get; set; }
	}
}
