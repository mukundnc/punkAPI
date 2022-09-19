using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PunkModels;
using PunkService;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PunkUnitTests
{
    [TestClass]
    public class RatingsRepoTest
    {
        [TestMethod]
        public async Task ValidateFileWriteAsync()
        {
            var filePath = $"{Environment.CurrentDirectory}/tesfile.json";
            var repoSettings = new RepoSettings() { FilePath = filePath };
            var logger = new MockLogger<RatingsRepo>();
            var options = new Mock<IOptions<RepoSettings>>();
            options.SetupGet(x => x.Value).Returns(repoSettings);
            var beerId = 2;
            var rating = new UserRating() { Comments = "comments", Rating = 2.5f, Username = "dummy@dummy.com" };
            var repo = new RatingsRepo(logger, options.Object);
            var addResult = await repo.AddRating(beerId, rating);
            Assert.IsTrue(addResult);
            var getResult = (await repo.GetRatings(beerId)).ToList();
            Assert.IsTrue(getResult.Count == 1);
            Assert.IsTrue(getResult[0].Comments == rating.Comments);
            Assert.IsTrue(getResult[0].Username == rating.Username);
            Assert.IsTrue(getResult[0].Rating == rating.Rating);
            File.Delete(filePath);
        }
    }
}
