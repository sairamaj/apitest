using System.IO;
using ApiManager.Model;
using ApiManager.Scripts.ViewModels;

namespace ApiManager.ViewModels
{
	class JsExecuteViewModel : InfoViewModel
	{
		public JsExecuteViewModel(JsScriptInfo jsScriptInfo) : base(null, jsScriptInfo)
		{
			this.JsScriptInfo = jsScriptInfo;
			if (File.Exists(jsScriptInfo.ScriptFileName))
			{
				this.ScriptFileContent = File.ReadAllText(jsScriptInfo.ScriptFileName);
				this.Script = new JavaScriptViewModel(this.ScriptFileContent);
			}
			else
			{
				this.Script = new JavaScriptViewModel($"{jsScriptInfo.ScriptFileName} not found!");
			}

			this.ScriptInfo = $"{Path.GetFileName(jsScriptInfo.ScriptFileName)} ({jsScriptInfo.Message})";
		}

		public JsScriptInfo JsScriptInfo{ get; }
		public string ScriptFileContent { get; set; }
		public JavaScriptViewModel Script { get; set; }
		public string ScriptInfo { get; set; }
	}
}
