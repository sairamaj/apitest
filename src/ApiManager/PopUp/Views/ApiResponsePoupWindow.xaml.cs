﻿using System.Reflection;
using System.Windows;
using System.Xml;

namespace ApiManager.PopUp.Views
{
	/// <summary>
	/// Interaction logic for ApiResponsePoupWindow.xaml
	/// </summary>
	public partial class ApiResponsePoupWindow : Window
	{
		public ApiResponsePoupWindow()
		{
			InitializeComponent();

			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "ApiManager.Views.AvalonJsonSyntax.xml";

			using (var xshd_stream = assembly.GetManifestResourceStream(resourceName))
			{
				var xshd_reader = new XmlTextReader(xshd_stream);
				this.RequestText.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd_reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
			}

			using (var xshd_stream = assembly.GetManifestResourceStream(resourceName))
			{
				var xshd_reader = new XmlTextReader(xshd_stream);
				this.ResponseText.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd_reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
			}
			using (var xshd_stream = assembly.GetManifestResourceStream(resourceName))
			{
				var xshd_reader = new XmlTextReader(xshd_stream);
				this.Response2Text.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd_reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
			}
		}
	}
}
