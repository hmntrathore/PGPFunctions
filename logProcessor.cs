using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace hmnt.Function
{
    public  class LogProcessor
    {
       public static void processLog (LogDetail logDetail)
       {
           var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://loggingfuncapp.azurewebsites.net/api/CustomLogHTTPClient?code=FbUa04KCkp52YM0NfvMSzxleC08lkVa3jsz4ezRHgJ/9q94KyX10CQ==");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
{
    string json =JsonConvert.SerializeObject(logDetail);

    streamWriter.Write(json);
}

var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
{
    var result = streamReader.ReadToEnd();
}
       }

    }
}