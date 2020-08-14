using System.Reflection;
using System.Windows;
using System.Xml;

namespace ApiManager.NewRequest.Views
{
	/// <summary>
	/// Interaction logic for NewRequestWindow.xaml
	/// </summary>
	public partial class NewRequestWindow : Window
	{
		public NewRequestWindow()
		{
			InitializeComponent();

			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "ApiManager.Views.AvalonJsonSyntax.xml";

			using (var xshd_stream = assembly.GetManifestResourceStream(resourceName))
			{
				var xshd_reader = new XmlTextReader(xshd_stream);
				this.RequestView.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd_reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
			}

			using (var xshd_stream = assembly.GetManifestResourceStream(resourceName))
			{
				var xshd_reader = new XmlTextReader(xshd_stream);
				this.ResponseView.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd_reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
			}
		}
	}
}
