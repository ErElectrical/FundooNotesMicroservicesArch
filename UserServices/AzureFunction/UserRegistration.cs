
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserRegistration.Model;

namespace UserRegistration
{
    public static class UserRegistration
    {

        [FunctionName("UserRegistration")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "FundooNotesDb",
                collectionName: "UserDetails",
                ConnectionStringSetting = "CosmosDBConnection")]
            IAsyncCollector<object> todos,
            ILogger log)
        {
            
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                var details = new UserDetails()
                {
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Email = data.Email,
                    Password = data.PassWord,
                    CreatedAt = DateTime.Now
                };

                await todos.AddAsync(details);

                return new OkObjectResult(details);
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
