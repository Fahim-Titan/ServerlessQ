using BusinessLogic.Interface;
using BusinessLogic.Service;
using DataAccess.Interface;
using DataAccess.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(ServerlessQ.Startup))]
namespace ServerlessQ
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IRepository, SQLiteDatabase>();
            //TODO: Move the connection string to the env
            builder.Services.AddSingleton<IMessageQueue>(sp =>
            {
                var connectionString = "UseDevelopmentStorage=true";
                var queueName = "my-queue";
                return new AzureDataAccess(connectionString, queueName);
            });
            builder.Services.AddSingleton<IBusinessLogic>(sp =>
            {
                var repo = sp.GetService<IRepository>();
                var msgQueue = sp.GetService<IMessageQueue>();
                var httpClient = new HttpClient();
                string apiUrl = "https://tagdiscovery.com/api/get-initials?name=";
                return new Business(repo, msgQueue, httpClient, apiUrl);
            });
        }
    }
}
