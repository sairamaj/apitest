namespace ApiManager.Model
{
	public class ErrorInfo :Info
	{
		public ErrorInfo()
		{
			this.Type = "Error";
		}

		public string Error { get; set; }
	}
}
