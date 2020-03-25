using ApiManager.Model;
using ApiManager.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ApiManager.Views
{
    class InfoDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ApiInfoTemplate { get; set; }
        public DataTemplate ExtractTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var viewModel = item as InfoViewModel;
            if (viewModel == null)
            {
                return ApiInfoTemplate;
            }

            switch (viewModel.Info.Type)
            {
                case "Extract":
                    return ExtractTemplate;
                default:
                    return ApiInfoTemplate;
            }
        }
    }
}
