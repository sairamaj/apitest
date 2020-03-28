using ApiManager.Model;
using ApiManager.Repository;

namespace ApiManager.ViewModels
{
	class ExtractVariableViewModel : InfoViewModel
	{
		public ExtractVariableViewModel(ICommandExecutor executor, ExtractVariableInfo extractVariableInfo) 
			: base(executor, extractVariableInfo)
		{
			this.ExtractVariableInfo = extractVariableInfo;
		}			

		public ExtractVariableInfo ExtractVariableInfo { get; set; }
	}
}
