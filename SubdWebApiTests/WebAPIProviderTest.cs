using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using subdDataProvider;
using System.Linq;

namespace SubdWebApiTests
{
    [TestClass]
    public class WebAPIProviderTest
    {
        private IMemory provider;

        [TestInitialize]
        public void SetupContext()
        {
            provider = new WebAPIProvider("Test2", "123", "http://localhost:58339");
        }

        [TestMethod]
        public void AddAuthorTest()
        {
            Authors authors = new Authors() { Name = "test", Surname = "test", MiddleName = "test", Email = "test@test", SecurityLabel = 2 };
            var result = provider.AddAuthorAsync(authors).Result;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetInfoTest()
        {
            var result = provider.GetInfoAsync().Result;
            Assert.AreEqual(result.Login, "test2");
        }

        [TestMethod]
        public void UpdateAuthorTest()
        {
            var auth = provider.GetAuthorsAsync(null).Result.First();
            auth.Name = "Updated";
            var result = provider.UpdateAuthorAsync(auth).Result;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanConnectTest()
        {
            Assert.IsTrue(provider.CanConnect);
        }

        [TestMethod]
        public void DeleteAuthorTest()
        {
            var auth = provider.GetAuthorsAsync(null).Result.First().Id;
            var result = provider.DeleteAuthorAsync(auth).Result;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ExecCommandTest()
        {
            var result = provider.ExecuteCommand("Select * from users").Result;
            var testresult = string.IsNullOrEmpty(result);
            Assert.IsTrue(testresult);
        }
    }
}
