using System.Collections.Generic;

namespace ApiViewer.Model
{
    public class Request
    {
        public string Body { get; set; }
        public IDictionary<string, string> Headers { get; set; }
    }
}
