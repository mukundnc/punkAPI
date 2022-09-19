using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using PunkModels;
using PunkService;
using RichardSzalay.MockHttp;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PunkUnitTests
{
    [TestClass]
    public class PunkProxyTest
    {
        [TestMethod]
        public async Task GetBeerByIdValidIdAsync()
        {
            var id = 2;
            var beer = new Beer() { Id = id, Name = "test", Description = "description" };
            var punk = "http://punk.api/*";
            var json = $"[{JsonConvert.SerializeObject(beer)}]";
            var logger = new MockLogger<PunkProxy>();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(punk).Respond(Application.Json, json);
            var client = new HttpClient(mockHttp);
            var options = new Mock<IOptions<ApiSettings>>();
            options.SetupGet(x => x.Value).Returns(new ApiSettings() { Punk= punk});
            var proxy = new PunkProxy(logger, client, options.Object);
            var result = await proxy.GetBeerById(id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == id);
            Assert.IsTrue(result.Name == beer.Name);
            Assert.IsTrue(result.Description == beer.Description);
        }

        [TestMethod]
        public async Task GetBeerByIdInValidIdAsync()
        {
            var id = 2;
            var punk = "http://punk.api/*";
            var logger = new MockLogger<PunkProxy>();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(punk).Respond(HttpStatusCode.BadRequest);
            var client = new HttpClient(mockHttp);
            var options = new Mock<IOptions<ApiSettings>>();
            options.SetupGet(x => x.Value).Returns(new ApiSettings() { Punk = punk });
            var proxy = new PunkProxy(logger, client, options.Object);
            var result = await proxy.GetBeerById(id);
            var beer = new Beer();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == beer.Id);
            Assert.IsTrue(result.Name == beer.Name);
            Assert.IsTrue(result.Description == beer.Description);
        }
        [TestMethod]
        public async Task GetBeerByIdExceptionAsync()
        {
            var id = 2;
            var punk = "http://punk.api/*";
            var message = "message";
            var logger = new MockLogger<PunkProxy>();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(punk).Throw(new Exception(message));
            var client = new HttpClient(mockHttp);
            var options = new Mock<IOptions<ApiSettings>>();
            options.SetupGet(x => x.Value).Returns(new ApiSettings() { Punk = punk });
            var proxy = new PunkProxy(logger, client, options.Object);
            try
            {
                var result = await proxy.GetBeerById(id);
                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == message);
            }
        }
        [TestMethod]
        public async Task GetBeersByNameValidIdAsync()
        {
            var name = "test";
            var beer = new Beer() { Id = 2, Name = name, Description = "description" };
            var punk = "http://punk.api/*";
            var json = $"[{JsonConvert.SerializeObject(beer)}]";
            var logger = new MockLogger<PunkProxy>();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(punk).Respond(Application.Json, json);
            var client = new HttpClient(mockHttp);
            var options = new Mock<IOptions<ApiSettings>>();
            options.SetupGet(x => x.Value).Returns(new ApiSettings() { Punk = punk });
            var proxy = new PunkProxy(logger, client, options.Object);
            var result = (await proxy.GetBeersByName(name)).ToList();
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0].Id == beer.Id);
            Assert.IsTrue(result[0].Name == beer.Name);
            Assert.IsTrue(result[0].Description == beer.Description);
        }

        [TestMethod]
        public async Task GetBeersByNameInValidIdAsync()
        {
            var name = "test";
            var punk = "http://punk.api/*";
            var logger = new MockLogger<PunkProxy>();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(punk).Respond(HttpStatusCode.BadRequest);
            var client = new HttpClient(mockHttp);
            var options = new Mock<IOptions<ApiSettings>>();
            options.SetupGet(x => x.Value).Returns(new ApiSettings() { Punk = punk });
            var proxy = new PunkProxy(logger, client, options.Object);
            var result = await proxy.GetBeersByName(name);
            Assert.IsTrue(result.ToList().Count == 0);
        }
        [TestMethod]
        public async Task GetBeersByNameExceptionAsync()
        {
            var name = "test";
            var punk = "http://punk.api/*";
            var message = "message";
            var logger = new MockLogger<PunkProxy>();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(punk).Throw(new Exception(message));
            var client = new HttpClient(mockHttp);
            var options = new Mock<IOptions<ApiSettings>>();
            options.SetupGet(x => x.Value).Returns(new ApiSettings() { Punk = punk });
            var proxy = new PunkProxy(logger, client, options.Object);
            try
            {
                var result = await proxy.GetBeersByName(name);
                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == message);
            }
        }
    }
}
