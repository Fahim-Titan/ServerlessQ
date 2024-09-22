using BusinessLogic.Interface;
using BusinessLogic.Service;
using DataAccess.Interface;
using DataAccess.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessQ
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IRepository, SQLiteDatabase>();
            builder.Services.AddSingleton<IMessageQueue, AzureDataAccess>();
            builder.Services.AddSingleton<IBusinessLogic>(sp =>
            {
                var repo = sp.GetService<IRepository>();
                var msgQueue = sp.GetService<IMessageQueue>();
                return new Business(repo, msgQueue);
            });
        }
    }
}
