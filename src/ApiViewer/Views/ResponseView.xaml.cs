using System.Reflection;
using System.Windows.Controls;
using System.Xml;

namespace ApiViewer.Views
{
    /// <summary>
    /// Interaction logic for ResponseView.xaml
    /// </summary>
    public partial class ResponseView : UserControl
    {
        public ResponseView()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ApiViewer.Views.AvalonJsonSyntax.xml";

            using (var xshd_stream = assembly.GetManifestResourceStream(resourceName))
            {
                var xshd_reader = new XmlTextReader(xshd_stream);
                this.TextEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd_reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
            }
        }
    }
}
