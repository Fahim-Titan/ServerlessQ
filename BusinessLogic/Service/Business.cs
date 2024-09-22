using BusinessLogic.Interface;
using DataAccess.Interface;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class Business: IBusinessLogic
    {
        private readonly IRepository _repo;
        private readonly IMessageQueue _msgQueue;
        public Business(IRepository dataAccess, IMessageQueue messageQueue)
        {
            _repo = dataAccess;
        }

        public Task PublishData(Person data)
        {
            _msgQueue.SendMessage(data);
            throw new NotImplementedException();
        }

        public Task SaveData(string firstName, string lastName)
        {
            //TODO: add validation

            Person person = new Person();
            person.FirstName = firstName;
            person.LastName = lastName;
            var dbData = _repo.SaveData(person);
            
            //TODO: Return result
            throw new NotImplementedException();
        }
    }
}
