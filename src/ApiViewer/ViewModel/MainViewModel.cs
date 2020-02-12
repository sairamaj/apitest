using System;
using System.Windows;
using ApiViewer.Pipes;
using Wpf.Util.Core.ViewModels;

namespace ApiViewer.ViewModel
{
    class MainViewModel : CoreViewModel
    {
        public MainViewModel(IMessageListener listener)
        {
            this.Text = "initial text";
            try
            {
                listener.SubScribe(s =>
                {
                    this.Text += s;
                    OnPropertyChanged(() => this.Text);
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public string Text { get; set; }
    }
}
