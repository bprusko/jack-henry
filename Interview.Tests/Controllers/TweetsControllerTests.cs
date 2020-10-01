using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Moq;
using Interview.Controllers;
using Interview.Services.Interfaces;
using Newtonsoft.Json;

namespace Interview.Tests.Controllers
{
    [TestClass]
    public class TweetsControllerTests
    {
        private Mock<ILogger<TweetsController>> _logger;
        private TweetsController _tweetStatisticsController;
        private Mock<ITweetStatisticsService> _tweetStatisticsService;

        [TestInitialize]
        public void InitTest()
        {
            _tweetStatisticsService = new Mock<ITweetStatisticsService>();
            _logger = new Mock<ILogger<TweetsController>>();
            _tweetStatisticsController = new TweetsController(_tweetStatisticsService.Object, _logger.Object);
        }

        [TestMethod]
        public void GetStatistics()
        {
            _tweetStatisticsService.Setup(s => s.GetStatistics()).Returns(TestData.TweetStats);
            var stats = _tweetStatisticsController.GetStatistics();
            Assert.AreEqual(JsonConvert.SerializeObject(stats), JsonConvert.SerializeObject(TestData.TweetStats));
            _tweetStatisticsService.Verify(mock => mock.GetStatistics(), Times.Once());

        }
    }
}