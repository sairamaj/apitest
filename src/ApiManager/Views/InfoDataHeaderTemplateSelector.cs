﻿using ApiManager.Model;
using ApiManager.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ApiManager.Views
{
    class InfoDataHeaderTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ApiInfoHeaderTemplate { get; set; }
        public DataTemplate ExtractHeaderTemplate { get; set; }
		public DataTemplate AssertHeaderTemplate { get; set; }
		public DataTemplate ErrorHeaderTemplate { get; set; }
		public DataTemplate ApiExecuteHeaderTemplate { get; set; }
		public DataTemplate JsScrptHeaderTemplate { get; set; }
        public DataTemplate PrintHeaderTemplate { get; set; }


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
				case "Assert":
					return AssertHeaderTemplate;
                case "Print":
                    return PrintHeaderTemplate;
                case "Error":
					return ErrorHeaderTemplate;
				case "ApiExecute":
					return ApiExecuteHeaderTemplate;
				case "JsExecute":
					return JsScrptHeaderTemplate;
				default:
                    return ApiInfoHeaderTemplate;
            }
        }
    }
}
