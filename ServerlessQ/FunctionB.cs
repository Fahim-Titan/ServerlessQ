using System;
using System.Threading.Tasks;
using BusinessLogic.Interface;
using DataAccess.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServerlessQ
{
    public class FunctionB
    {
        private readonly IBusinessLogic _logic;
        public FunctionB(IBusinessLogic logic)
        {
            _logic = logic;
        }
        [FunctionName("FunctionB")]
        public async Task Run([QueueTrigger("my-queue", Connection = "AzureWebJobsStorage")] Person person, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {person.FirstName}");
            try
            {
                var svg = await _logic.GetSVG(person);
                //await _logic.SaveSVG(person, svg);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error processing queue message");
            }
        }
    }
}
