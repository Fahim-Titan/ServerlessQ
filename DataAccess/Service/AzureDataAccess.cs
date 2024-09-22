using DataAccess.Interface;
using DataAccess.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public class AzureDataAccess: IMessageQueue
    {
        private readonly CloudQueue _queue;
        public AzureDataAccess(string connectionString, string queueName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference(queueName);
            _queue.CreateIfNotExistsAsync().Wait();
        }

        public async Task SendMessage(Person person)
        {
            var message = new CloudQueueMessage(JsonConvert.SerializeObject(person));
            await _queue.AddMessageAsync(message);
        }
    }
}
