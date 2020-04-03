namespace ApiManager.Model
{
	class JsScriptInfo : Info
	{
		public JsScriptInfo()
		{
			this.Type = "JsExecute";
		}

		public string ScriptFileName { get; set; }
		public string Error { get; set; }
	}
}
