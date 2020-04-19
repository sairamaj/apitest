using System;
using Microsoft.Azure.Cosmos.Table;

namespace ApiRunner
{
    public class RunEntity : TableEntity
    {
        public string Name {get; set;}
        public DateTime DateTime {get; set;}
        public RunStatus Status {get; set;}
        public string Message {get; set;}
    }
}