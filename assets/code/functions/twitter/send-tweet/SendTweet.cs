using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

namespace AzureLowlands.Functions.Twitter
{
    public static class SendTweet
    {
        private static IConfigurationRoot _configuration;

        public static IConfigurationRoot Configuration { get => _configuration; set => _configuration = value; }

        [FunctionName("SendTweet")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ExecutionContext context, ILogger log)
        {
            try
            {
                log.LogInformation("SendTweet Function triggered.");

                Configuration = new ConfigurationBuilder()
                                        .SetBasePath(context.FunctionAppDirectory)
                                        // This gives you access to your application settings in your local development environment
                                        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                                        // This is what actually gets you the application settings in Azure
                                        .AddEnvironmentVariables()
                                        .Build();

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                if(string.IsNullOrWhiteSpace(data?.message?.ToString()))
                {
                    return new BadRequestObjectResult("Missing message parameter");
                }

                LinkedIn.PublishMessage(data?.message.ToString(), data?.imageUrl?.ToString());

                return new OkResult();
            }
            catch(Exception exception)
            {
                return new BadRequestObjectResult(exception);
            }
        }
    }
}
