using DataAccess.Interface;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    internal class AzureDataAccess: IDataAccess
    {
        public AzureDataAccess() { }

        public Task SaveData(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
