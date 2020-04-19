using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;

namespace site.Models
{
    public class RunEntity : TableEntity
    {
        public RunEntity()
        {
            this.Apis = new List<ApiInfoEntity>();
        }
        
        public string Id => this.PartitionKey;
        public string Name {get; set;}
        public DateTime DateTime {get; set;}
        public RunStatus Status {get; set;}
        public string Message {get; set;}

        public IList<ApiInfoEntity> Apis {get; set;}
    }
}