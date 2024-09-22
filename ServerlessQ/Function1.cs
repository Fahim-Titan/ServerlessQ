using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BusinessLogic.Interface;

namespace ServerlessQ
{
    public class Function1
    {
        private readonly IBusinessLogic _logic;
        public Function1(IBusinessLogic logic)
        {
            _logic = logic;
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string firstName = req.Query["FirstName"];
                string lastName = req.Query["LastName"];

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                firstName = firstName ?? data?.firstName;
                lastName = lastName ?? data?.lastName;

                string responseMessage = string.IsNullOrEmpty(firstName)
                    ? "This HTTP triggered function executed successfully."
                    : $"Hello, {firstName}. This HTTP triggered function executed successfully.";

                // TODO: call the business logic to save data
                var person = await _logic.SaveData(firstName, lastName);
                // TODO: Get the data
                // TODO: publish to the Azure Q
                await _logic.PublishData(person);

                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }
    }
}
