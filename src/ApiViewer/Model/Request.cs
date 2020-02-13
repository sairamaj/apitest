using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiViewer.Model
{
    public class Request
    {
        public string Body { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string HeadersAsString
        {
            get
            {
                if (this.Headers == null)
                {
                    return string.Empty;
                }

                return JsonConvert.SerializeObject(this.Headers, Formatting.Indented);
            }
        }

        public string RequestData
        {
            get
            {
                return $"Headers:\r\n{this.HeadersAsString}\r\nBody:\r\n{this.Body}";
            }
            set
            {

            }
        }
    }
}
