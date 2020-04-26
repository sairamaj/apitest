using System.Windows;
using System.Windows.Controls;
using ApiManager.ScenarioEditing.ViewModel;

namespace ApiManager.ScenarioEditing
{
	class ScenarioItemDataHeaderTemplateSelector : DataTemplateSelector
	{
		public DataTemplate ApiHeaderTemplate { get; set; }
		public DataTemplate CommentHeaderTemplate { get; set; }
		public DataTemplate CommandHeaderTemplate { get; set; }
		public DataTemplate LineBreakHeaderTemplate { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if( item is ScenarioLineItemViewModel scenarioLineItemViewModel )
			{
				switch (scenarioLineItemViewModel.LineItem.Type)
				{
					case "comment":
						return CommentHeaderTemplate;
					case "command":
						return CommandHeaderTemplate;
					case "linebreak":
						return LineBreakHeaderTemplate;
					default:
						return ApiHeaderTemplate;
				}
			}

			return ApiHeaderTemplate;
		}
	}
}
