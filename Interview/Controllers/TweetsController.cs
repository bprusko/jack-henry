using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Interview.Services.Interfaces;
using Interview.Models.Statistics;
using Newtonsoft.Json;

namespace Interview.Controllers
{
    [ApiController]
    [Route("api/v1/tweets/")]
    public class TweetsController : ControllerBase
    {
        private ITweetStatisticsService _tweetStatisticsService;
        private readonly ILogger<TweetsController> _logger;

        public TweetsController(ITweetStatisticsService tweetStatisticsService, ILogger<TweetsController> logger) {
            _logger = logger;
            _tweetStatisticsService = tweetStatisticsService;
        }

        [HttpGet]
        [Route("statistics")]
        public TweetStatistics GetStatistics()
        {
            var stats = _tweetStatisticsService.GetStatistics();
            Console.WriteLine(JsonConvert.SerializeObject(stats));
            return stats;
        }
    }
}
