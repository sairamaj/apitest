﻿using System.Windows;
using System.Windows.Controls;
using ApiManager.ViewModels;

namespace ApiManager.Views
{
	public class InfoDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ApiDataTemplate { get; set; }
        public DataTemplate ExtractDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var viewModel = item as InfoViewModel;
            if (viewModel == null)
            {
                return ApiDataTemplate;
            }

            switch (viewModel.Info.Type)
            {
                case "Extract":
                    return ExtractDataTemplate;
                default:
                    return ApiDataTemplate;
            }
        }
    }
}
