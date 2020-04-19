using System;
using Microsoft.Azure.Cosmos.Table;

namespace ApiRunner
{
    public class ApiInfoEntity : TableEntity
    {
        public ApiInfoEntity()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public int HttpCode { get; set; }
        public string StatusCode { get; set; }
        public string Data {get; set;}

        public override string ToString()
        {
            return $"{this.Method} {this.HttpCode} {this.Url}";
        }
    }
}