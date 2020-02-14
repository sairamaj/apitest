﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ApiViewer.Pipes;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiViewer.ViewModel
{
    class MainViewModel : CoreViewModel
    {
        private ApiInfoViewModel _seleApiInfoViewModel;

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

            this.ClearCommand = new DelegateCommand(() =>
            {
                this.ApiInfoViewModels.Clear();
            });
        }
        public ObservableCollection<ApiInfoViewModel> ApiInfoViewModels { get; set; }
        public ICommand ClearCommand { get; set; }

        public ApiInfoViewModel SelectedApiInfoViewModel
        {
            get => _seleApiInfoViewModel;
            set
            {
                this._seleApiInfoViewModel = value;
                this.CurrentRequestViewModel = new RequestViewModel(this._seleApiInfoViewModel.ApiInfo.Request);
                this.CurrentResponseViewModel = new ResponseViewModel(this._seleApiInfoViewModel.ApiInfo.Response);

                OnPropertyChanged(()=> this.CurrentRequestViewModel);
                OnPropertyChanged(() => this.CurrentResponseViewModel);
                //this.CurrentRequestViewModel.Update();
                //this.CurrentResponseViewModel.Update();
            }
        }

        public RequestViewModel CurrentRequestViewModel { get; set; }
        public ResponseViewModel CurrentResponseViewModel { get; set; }
    }
}
