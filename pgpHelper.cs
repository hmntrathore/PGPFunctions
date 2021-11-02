using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PgpCore;
using System.Text;
namespace hmnt.Function
{
    public static class PGPHelper
    {
        
        public static  string GetStringFromBase64(string base64String)
        {           
            byte[] publicKeyBytes = Convert.FromBase64String(base64String);          
            return Encoding.UTF8.GetString(publicKeyBytes);
        }
        public static async Task<Stream> EncryptAsync(Stream inputStream, string publicKey)
        {
            using (PGP pgp = new PGP())
            {
                Stream outputStream = new MemoryStream();

                using (inputStream)
                using (Stream publicKeyStream = GenerateStreamFromString(publicKey))
                {
                    await pgp.EncryptStreamAsync(inputStream, outputStream, publicKeyStream, true, true);
                    outputStream.Seek(0, SeekOrigin.Begin);
                    return outputStream;
                }
            }
        }

          public static async Task<Stream> SignAsync(Stream inputStream, string privateKey, string passPhrase)
        {
            using (PGP pgp = new PGP())
            {
                Stream outputStream = new MemoryStream();

                using (inputStream)
                using (Stream publicKeyStream = GenerateStreamFromString(privateKey))
                {
                   await pgp.SignStreamAsync(inputStream,outputStream,publicKeyStream,passPhrase,true,true);                 
                    outputStream.Seek(0, SeekOrigin.Begin);
                    return outputStream;
                }
            }
        }

          public static async Task<Stream> EncryptAndSignAsync(Stream inputStream, string publicKey, string privateKey, string passPhrase)
        {
            using (PGP pgp = new PGP())
            {
                Stream outputStream = new MemoryStream();

                using (inputStream)
                using (Stream publicKeyStream = GenerateStreamFromString(publicKey))
                {
                     using (Stream privateKeyStream = GenerateStreamFromString(privateKey))
                {
                   await pgp.EncryptStreamAndSignAsync (inputStream,outputStream,publicKeyStream,privateKeyStream, passPhrase,true,true);                 
                    outputStream.Seek(0, SeekOrigin.Begin);
                    return outputStream;
                }
                }
            }
        }

        

        public static async Task<Stream> DecryptAsync(Stream inputStream, string privateKey, string passPhrase)
        {
            using (PGP pgp = new PGP())
            {
                Stream outputStream = new MemoryStream();

                using (inputStream)
                using (Stream privateKeyStream = GenerateStreamFromString(privateKey))
                {
                    await pgp.DecryptStreamAsync(inputStream,outputStream,privateKeyStream,passPhrase);
                  
                    outputStream.Seek(0, SeekOrigin.Begin);
                    return outputStream;
                }
            }
        }

         public static async Task<Stream> DecryptAndVerifyAsync(Stream inputStream, string publicKey ,string privateKey, string passPhrase)
        {
            using (PGP pgp = new PGP())
            {
                Stream outputStream = new MemoryStream();

                using (inputStream)
                using (Stream privateKeyStream = GenerateStreamFromString(privateKey))
                {
                 using (Stream publicKeyStream = GenerateStreamFromString(publicKey))
                {
                    await pgp.DecryptStreamAndVerifyAsync(inputStream,outputStream,publicKeyStream,privateKeyStream,passPhrase);
                  
                    outputStream.Seek(0, SeekOrigin.Begin);
                    return outputStream;
                }
                }
            }
        }

          public static async Task<Stream> VerifyAsync(Stream inputStream, string publicKey)
        {
            using (PGP pgp = new PGP())
            {
               
                string resp= "Invalid Signature";

                using (inputStream)
                using (Stream publicKeyStream = GenerateStreamFromString(publicKey))
                {
                    bool resut =  await pgp.VerifyStreamAsync(inputStream,publicKeyStream);
                    if(resut)
                    resp ="Signature Verified Succesfully";
                    return GenerateStreamFromString(resp);
                    
                }
            }
        }



        private static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}