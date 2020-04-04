using System.IO;
using ApiManager.Model;

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
			}

			this.ScriptInfo = $"{Path.GetFileName(jsScriptInfo.ScriptFileName)} ({jsScriptInfo.Message})";
		}

		public JsScriptInfo JsScriptInfo{ get; }
		public string ScriptFileContent { get; set; }
		public string ScriptInfo { get; set; }
	}
}
