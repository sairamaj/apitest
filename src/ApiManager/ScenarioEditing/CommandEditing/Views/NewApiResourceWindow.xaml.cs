using System.Reflection;
using System.Windows;
using System.Xml;

namespace ApiManager.ScenarioEditing.CommandEditing.Views
{
	/// <summary>
	/// Interaction logic for NewApiResourceWindow.xaml
	/// </summary>
	public partial class NewApiResourceWindow : Window
	{
		public NewApiResourceWindow()
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
