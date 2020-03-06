using System;
using System.Collections.Generic;
using System.Net;

namespace ApiViewer.Model
{
    public class ApiInfo
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public string StatusCode { get; set; }
        public long TimeTaken { get; set; }
        public Request Request { get; set; }
        public Response Response { get; set; }

        public string RelativeUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.Url))
                {
                    return string.Empty;
                }

                return new Uri(this.Url).PathAndQuery;
            }
        }

        public string TimeTakenString
        {
            get
            {
                var timeTakenMilliseconds = (long)(((double)this.TimeTaken / 1000.0));
                return timeTakenMilliseconds.ToString();
            }
        }
    }
}
