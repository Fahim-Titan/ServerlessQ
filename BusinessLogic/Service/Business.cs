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
        private readonly HttpClient _httpClient;
        public Business(IRepository dataAccess, IMessageQueue messageQueue, HttpClient httpClient)
        {
            _repo = dataAccess;
            _msgQueue = messageQueue;
            _httpClient = httpClient;
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

        public async Task<string> GetSVG(Person person)
        {
            try
            {
                var fullName = $"{person.FirstName}{person.LastName}";
                var response = await _httpClient.GetAsync($"https://tagdiscovery.com/api/get-initials?name={fullName}");
                response.EnsureSuccessStatusCode();
                var svg = await response.Content.ReadAsStringAsync();
                return svg;
            }
            catch (Exception ex)
            {
                // add logger
                throw new Exception("Error Occured in API request to fetch svg data", ex);
            }
        }

        public async Task<bool> SaveSVG(int personId, string svg)
        {
            try
            {
                var person = await _repo.GetPerson(personId);
                if(person == null)
                {
                    throw new Exception("Can not find the person with the given ID value");
                }
                var result = await _repo.SaveSvgData(person, svg);
                return result == null ? false : true;
            }
            catch (Exception ex)
            {
                // add logger
                throw;
            }
        }
    }
}
