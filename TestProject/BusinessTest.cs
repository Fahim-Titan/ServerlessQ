using BusinessLogic.Service;
using DataAccess.Interface;
using DataAccess.Model;
using Moq;

namespace TestProject
{
    [TestFixture]
    public class BusinessTests
    {
        private Mock<IRepository> _mockRepo;
        private Mock<IMessageQueue> _mockMsgQueue;
        private HttpClient _httpClient;
        private Business _business;
        private string apiUrl = "mockUrl=";

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository>();
            _mockMsgQueue = new Mock<IMessageQueue>();
            _httpClient = new HttpClient();

            _business = new Business(_mockRepo.Object, _mockMsgQueue.Object, _httpClient, apiUrl);
        }

        [Test]
        public async Task SaveData_UniqueNamePair_ReturnsPerson()
        {
            var firstName = "John";
            var lastName = "Doe";
            var person = new Person { FirstName = firstName, LastName = lastName, Svg = String.Empty };
            _mockRepo.Setup(repo => repo.GetPersonByFirstAndLastName(firstName, lastName)).ReturnsAsync((Person)null);
            _mockRepo.Setup(repo => repo.SaveData(It.Is<Person>(p => p.FirstName == firstName && p.LastName == lastName)))
             .ReturnsAsync(person);

            var result = await _business.SaveData(firstName, lastName);

            Assert.That(result.FirstName == person.FirstName, Is.True);
            Assert.That(result.LastName == person.LastName, Is.True);
        }

        [Test]
        public void SaveData_NonUniqueNamePair_ThrowsException()
        {
            var firstName = "John";
            var lastName = "Doe";
            var person = new Person { FirstName = firstName, LastName = lastName, Svg = String.Empty };

            _mockRepo.Setup(repo => repo.GetPersonByFirstAndLastName(firstName, lastName)).ReturnsAsync(person);

            Assert.ThrowsAsync<Exception>(async () => await _business.SaveData(firstName, lastName));
        }
    }
}