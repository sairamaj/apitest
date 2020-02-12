using System;
using System.Collections.ObjectModel;
using System.Windows;
using ApiViewer.Pipes;
using Wpf.Util.Core;
using Wpf.Util.Core.ViewModels;

namespace ApiViewer.ViewModel
{
    class MainViewModel : CoreViewModel
    {

        public MainViewModel(IMessageListener listener)
        {
            this.ApiInfoViewModels = new SafeObservableCollection<ApiInfoViewModel>();
            try
            {
                listener.SubScribe(s =>
                {
                    // OnPropertyChanged(() => this.Text);
                    this.ApiInfoViewModels.Add(new ApiInfoViewModel(s));
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public ObservableCollection<ApiInfoViewModel> ApiInfoViewModels { get; set; }
    }
}
