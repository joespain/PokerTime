using NUnit.Framework;
using System.Net.Http;

namespace PokerTime.IntegrationTests
{
    public class APIUsersTest
    {
        private readonly HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            var appFactory = new WebApplicationFactory<>

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}