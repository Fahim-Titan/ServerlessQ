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
            _msgQueue = messageQueue;
        }

        public Task<byte[]> GetSVG(Person person)
        {
            throw new NotImplementedException();
        }

        public async Task<Person> SaveData(string firstName, string lastName)
        {
            //TODO: add validation

            Person person = new Person();
            person.FirstName = firstName;
            person.LastName = lastName;
            var dbData = await _repo.SaveData(person);

            return dbData;
        }

        public async Task<bool> PublishData(Person data)
        {
            try
            {
                await _msgQueue.SendMessage(data);
                return true;
            }
            catch (Exception ex)
            {
                //log here
                return false;
            }
        }
    }
}
