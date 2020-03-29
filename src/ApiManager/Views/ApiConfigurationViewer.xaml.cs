using System.Reflection;
using System.Windows;
using System.Xml;

namespace ApiManager.Views
{
	/// <summary>
	/// Interaction logic for ApiConfigurationViewer.xaml
	/// </summary>
	public partial class ApiConfigurationViewer : Window
	{
		public ApiConfigurationViewer()
		{
			InitializeComponent();

			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "ApiManager.Views.AvalonJsonSyntax.xml";

			using (var xshd_stream = assembly.GetManifestResourceStream(resourceName))
			{
				var xshd_reader = new XmlTextReader(xshd_stream);
				this.TextEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd_reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
			}

		}
	}
}
