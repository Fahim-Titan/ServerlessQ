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
        Task<Person> SaveData(string firstName, string lastName);
        Task<bool> PublishData(Person data);
        Task<string> GetSVG(Person person);
        Task<bool> SaveSVG(int personId, string svg);
    }
}
