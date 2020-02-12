﻿using System;
using System.Collections.Generic;
using System.Net;

namespace ApiViewer.Model
{
    public class ApiInfo
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public HttpStatusCode StatusCode { get; set; } 
        public int TimeTakenInMilliseconds { get; set; }

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
    }
}