using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using UserRegistration.Model;
using System.Linq;
using UserRegistration.Authorisation;
using UserServices.Authorisation;

namespace UserRegistration.AzureFunctions
{
    public class ForgetPassWord
    {
        GenrateToken auth = new GenrateToken();

        [FunctionName("ForgetPassWord")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                ConnectionStringSetting = "CosmosDBConnection"
                )]
                DocumentClient client,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request for ForgetPassWord");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<ForgetPasswordDetails>(requestBody);

                var option = new FeedOptions { EnableCrossPartitionQuery = true };
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri("FundooNotesDb", "UserDetails");
                var document = client.CreateDocumentQuery<UserDetails>(collectionUri, option).Where(t => t.Email == data.Email)
                        .AsEnumerable().FirstOrDefault();
                if (document != null)
                {
                    var token = this.auth.IssuingToken(document.Id.ToString());
                    new MsMq().Sender(token);
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