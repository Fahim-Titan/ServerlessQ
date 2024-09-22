using DataAccess.Interface;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public class AzureDataAccess: IMessageQueue
    {
        public AzureDataAccess() { }
        public Task SendMessage(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
