using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    public interface IRepository
    {
        Task<Person> SaveData(Person person);
        Task<Person> GetPerson(int id);
        Task<Person?> GetPersonByFirstAndLastName(string firstName, string lastName);
        Task<Person> SaveSvgData(Person person, string svg);
    }
}
