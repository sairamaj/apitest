using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiViewer.Model
{
    public class Response
    {
        public string Content { get; set; }
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

        public string ResponseData
        {
            get
            {
                return $"Headers:\r\n{this.HeadersAsString}\r\nContent:\r\n{this.Content}";
            }
            set
            {

            }
        } 

    }
}
