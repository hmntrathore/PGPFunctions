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
    public static class PGPVerify
    {
        [FunctionName("PGPVerify")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,  "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function {nameof(PGPVerify)} received a request.");
             req.Headers.TryGetValue("correlationID", out var correlationID);
            string reqType = req.Query["req"];
            string publicKey = PGPHelper.GetStringFromBase64(Environment.GetEnvironmentVariable("PublicKeyBase64"));           
            req.EnableBuffering(); //Make RequestBody Stream seekable
            Stream encryptedData ;            
            encryptedData = await PGPHelper.VerifyAsync(req.Body, publicKey);               
            return new OkObjectResult(encryptedData);
        }
    }
}
