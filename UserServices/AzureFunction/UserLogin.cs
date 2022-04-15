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
using System.Collections.Generic;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using UserRegistration.Authorisation;

namespace UserRegistration.AzureFunctions
{
    public class UserLogin
    {
        GenrateToken auth = new GenrateToken();

        [FunctionName("UserLogin")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
                        [CosmosDB(
                ConnectionStringSetting = "CosmosDBConnection"
                )]
                DocumentClient client,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var updated = JsonConvert.DeserializeObject<LoginDetails>(requestBody);

                var option = new FeedOptions { EnableCrossPartitionQuery = true };

                Uri collectionUri = UriFactory.CreateDocumentCollectionUri("FundooNotesDb", "UserDetails");
                var document = client.CreateDocumentQuery<UserDetails>(collectionUri, option).Where(t => t.Email == updated.Email && t.Password == updated.Password)
                        .AsEnumerable().FirstOrDefault();
                if (document != null)
                {
                    var token = auth.IssuingToken(document.Id.ToString());
                    return new OkObjectResult(token);

                }
                return (ActionResult)new NotFoundResult();
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }


        }
    }
}