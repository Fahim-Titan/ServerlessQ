using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessLogic.Interface
{
    public interface IBusinessLogic
    {
        Task SaveData(string firstName, string lastName);
        Task PublishData(Person data);
    }
}
