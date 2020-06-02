using System.Reflection;
using System.Windows.Controls;
using System.Xml;

namespace ApiManager.Resources.Views
{
	/// <summary>
	/// Interaction logic for ResourceWithSaveView.xaml
	/// </summary>
	public partial class ResourceWithSaveView : UserControl
	{
		public ResourceWithSaveView()
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
