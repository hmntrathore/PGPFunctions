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
    public static class PGPVerifyndDecrypt
    {
        [FunctionName("PGPVerifyndDecrypt")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
           log.LogInformation($"C# HTTP trigger function {nameof(PGPVerifyndDecrypt)} received a request.");
           req.Headers.TryGetValue("correlationID", out var correlationID);

            string publicKey = PGPHelper.GetStringFromBase64(Environment.GetEnvironmentVariable("PublicKeyBase64"));
            string privateKey = PGPHelper.GetStringFromBase64(Environment.GetEnvironmentVariable("PrivateKeyBase64"));
            string passPhrase = Environment.GetEnvironmentVariable("passPhrase");
            req.EnableBuffering(); //Make RequestBody Stream seekable
            Stream encryptedData ;          
            encryptedData = await PGPHelper.DecryptAndVerifyAsync(req.Body,publicKey, privateKey,passPhrase); 
            return new OkObjectResult(encryptedData);        }
    }
}
