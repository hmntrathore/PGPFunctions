using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace hmnt.Function
{
    public  class LogDetail
    {
        public string CorrelationID {get; set;}
        public DateTime LogDateTime {get; set;}
        // public string InstanceID {get; set;}
        // public string InstanceName {get; set;}

        // public string Message {get; set;}
        // public string status {get; set;}

        public string LogKey {get; set;}
        public string LogType {get; set;}
        public string LogValue {get; set;}
        public string ParentCorrelationID {get; set;}

    }
}