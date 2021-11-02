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
    public static class PGPSign
    {
        [FunctionName("PGPSign")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,  "post", Route = null)] HttpRequest req,
            ILogger log)
         {
            log.LogInformation($"C# HTTP trigger function {nameof(PGPSign)} received a request.");
           
            req.Headers.TryGetValue("correlationID", out var correlationID);
            log.LogInformation($"C# HTTP trigger function {nameof(PGPSign)}correlation value {correlationID}");
           
            string privateKey = PGPHelper.GetStringFromBase64(Environment.GetEnvironmentVariable("SourcePrivateKeyBase64"));
            string passPhrase = Environment.GetEnvironmentVariable("passPhrase");
            req.EnableBuffering(); //Make RequestBody Stream seekable
            Stream encryptedData ;
           
            encryptedData = await PGPHelper.SignAsync(req.Body, privateKey,passPhrase);            
                  


            return new OkObjectResult(encryptedData);
        }

    }
}
