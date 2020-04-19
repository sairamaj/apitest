using System;
using Microsoft.Azure.Cosmos.Table;

namespace ApiRunner
{
    public class RunEntity : TableEntity
    {
        public string Name {get; set;}
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
    }
}