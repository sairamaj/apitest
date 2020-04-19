using System;
using Microsoft.Azure.Cosmos.Table;

namespace site.Models
{
    public class ApiInfoEntity : TableEntity
    {
        public ApiInfoEntity()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get;}
        public string Method { get; set; }
        public string Url { get; set; }
        public string Path
        {
            get
            {
                return new Uri(this.Url).PathAndQuery;
            }
        }
        public int HttpCode { get; set; }
        public string StatusCode { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return $"{this.Method} {this.HttpCode} {this.Url}";
        }

        public bool Success => this.HttpCode >=200 && this.HttpCode <=299;
    }
}