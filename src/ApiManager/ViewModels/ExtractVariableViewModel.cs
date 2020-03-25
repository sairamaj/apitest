using ApiManager.Model;

namespace ApiManager.ViewModels
{
	class ExtractVariableViewModel : InfoViewModel
	{
		public ExtractVariableViewModel(ExtractVariableInfo extractVariableInfo) : base(extractVariableInfo)
		{
			this.ExtractVariableInfo = extractVariableInfo;
		}			

		public ExtractVariableInfo ExtractVariableInfo { get; set; }
	}
}
