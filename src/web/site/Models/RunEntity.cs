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
        public string Name => this.RowKey;
        public DateTime DateTime {get; set;}
        // Enums are not saved to azure (without using some workarounds)
        public string StatusInfo {
            get{
                return Status.ToString();
            }
            set{
                if(Enum.TryParse(typeof(RunStatus), value,true, out var val)){
                    this.Status = (RunStatus)val;
                }   
            }
        }
        public RunStatus Status {get; set;}
        public string Message {get; set;}

        public IList<ApiInfoEntity> Apis {get; set;}
    }
}