using System.Collections.Generic;

namespace ApiViewer.Model
{
    public class Response
    {
        public string Body { get; set; }
        public IDictionary<string, string> Headers { get; set; }
    }
}
