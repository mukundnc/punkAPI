using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PunkModels;
using PunkService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PunkUnitTests
{
    [TestClass]
    public class BeerServiceTest
    {

        [TestMethod]
        public async Task GetBeersWithValidNameAsync()
        {
            string name = "test";
            int id = 2;
            string comment = "comment";
            float rating = 2.5f;
            string username = "wdwd@wdwd.cc";
            var logger = new MockLogger<BeerService>();
            var punkProxyMock = new Mock<IPunkProxy>();
            punkProxyMock.Setup(p => p.GetBeersByName(name)).ReturnsAsync(new List<Beer> { new Beer() { Id = id, Name = name } });
            var ratingsRepoMock = new Mock<IRatingsRepo>();
            ratingsRepoMock.Setup(r => r.GetRatings(id)).ReturnsAsync(new List<UserRating> { new UserRating() { Comments = comment, Rating = rating, Username = username } });
            var service = new BeerService(logger, ratingsRepoMock.Object, punkProxyMock.Object);
            var result = await service.GetBeers(name);
            Assert.IsTrue(result.ToList().Count == 1);
            Assert.IsTrue(result.ToList()[0].Ratings.ToList().Count == 1);
        }

        [TestMethod]
        public async Task AddUserRatingWithValidNameAsync()
        {
            string name = "test";
            int id = 2;
            string comment = "comment";
            float rating = 2.5f;
            string username = "wdwd@wdwd.cc";
            var logger = new MockLogger<BeerService>();
            var punkProxyMock = new Mock<IPunkProxy>();
            var ratingsRepoMock = new Mock<IRatingsRepo>();
            ratingsRepoMock.Setup(r => r.AddRating(id, It.IsAny<UserRating>())).ReturnsAsync(true);
            ratingsRepoMock.Setup(r => r.GetRatings(id)).ReturnsAsync(new List<UserRating> { new UserRating() { Comments = comment, Rating = rating, Username = username } });
            var service = new BeerService(logger, ratingsRepoMock.Object, punkProxyMock.Object);
            var result = await service.AddUserRating(id, new UserRating() { Comments = comment, Rating = rating, Username = username });
            Assert.IsTrue(result.ToList().Count == 1);
            Assert.IsTrue(result.ToList()[0].Comments == comment);
            Assert.IsTrue(result.ToList()[0].Rating == rating);
            Assert.IsTrue(result.ToList()[0].Username == username);
        }

        [TestMethod]
        public async Task AddUserRatingWithExceptionAsync()
        {
            string name = "test";
            int id = 2;
            string comment = "comment";
            float rating = 2.5f;
            string username = "wdwd@wdwd.cc";
            var logger = new MockLogger<BeerService>();
            var punkProxyMock = new Mock<IPunkProxy>();
            var ratingsRepoMock = new Mock<IRatingsRepo>();
            ratingsRepoMock.Setup(r => r.AddRating(id, It.IsAny<UserRating>())).ReturnsAsync(false);
            var service = new BeerService(logger, ratingsRepoMock.Object, punkProxyMock.Object);
            try
            {
                var result = await service.AddUserRating(id, new UserRating() { Comments = comment, Rating = rating, Username = username });
                Assert.IsTrue(false);
            }
            catch(Exception ex)
            {
                Assert.IsTrue(ex.Message == Constants.AddFailedError);
            }
        }
    }
    public class MockLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            Console.WriteLine("test");
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine("test");
        }
    }
}

