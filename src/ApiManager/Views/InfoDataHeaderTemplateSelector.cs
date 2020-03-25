using ApiManager.Model;
using ApiManager.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ApiManager.Views
{
    class InfoDataHeaderTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ApiInfoHeaderTemplate { get; set; }
        public DataTemplate ExtractHeaderTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var viewModel = item as InfoViewModel;
            if (viewModel == null)
            {
                return ApiInfoHeaderTemplate;
            }

            switch (viewModel.Info.Type)
            {
                case "Extract":
                    return ExtractHeaderTemplate;
                default:
                    return ApiInfoHeaderTemplate;
            }
        }
    }
}
