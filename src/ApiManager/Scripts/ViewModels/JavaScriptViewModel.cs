namespace ApiManager.Scripts.ViewModels
{
	class JavaScriptViewModel
	{
		public JavaScriptViewModel(string content, string fileName)
		{
			this.Content = content;
			this.FileName = fileName;
		}

		public string Content { get; set; }
		public string FileName { get; set; }
	}
}
