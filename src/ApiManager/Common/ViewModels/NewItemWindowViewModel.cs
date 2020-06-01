using System;
using System.Windows;
using ApiManager.ViewModels;

namespace ApiManager.Common.ViewModels
{
	class NewItemWindowViewModel : DialogViewModel
	{
		private readonly Func<string, bool> _verifier;

		public NewItemWindowViewModel(Window win, string title, Func<string, bool> verifier) : base(win)
		{
			this.Title = title;
			this._verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
		}

		public string Title { get; }
		public string Name { get; set; }

		protected override bool OnClosing()
		{
			if (string.IsNullOrWhiteSpace(this.Name))
			{
				MessageBox.Show("Name cannot be null or empty.");
				return false;
			}

			return this._verifier(this.Name);
		}
	}
}
