using System.IO;
using ApiManager.Model;

namespace ApiManager.ViewModels
{
	class JsExecuteViewModel : InfoViewModel
	{
		public JsExecuteViewModel(JsScriptInfo jsScriptInfo) : base(null, jsScriptInfo)
		{
			this.JsScriptInfo = jsScriptInfo;
			this.ScriptFileContent = File.ReadAllText(jsScriptInfo.ScriptFileName);
		}

		public JsScriptInfo JsScriptInfo{ get; }
		public string ScriptFileContent { get; set; }
	}
}
