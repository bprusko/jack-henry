using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Interview.Services.Interfaces;

namespace Interview.Services
{
    public class TweetStatisticsBackgroundService : BackgroundService
    {
        private ITweetStatisticsService _tweetStatisticsService;
        public TweetStatisticsBackgroundService(ITweetStatisticsService tweetStatisticsService) {
            _tweetStatisticsService = tweetStatisticsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("[TWEETS STATISTICS BACKGROUND SERVICE][Starting]");

            while (!stoppingToken.IsCancellationRequested)
            {
                await _tweetStatisticsService.UpdateStatisticsAsync();
            }

            Console.WriteLine("[TWEETS STATISTICS BACKGROUND SERVICE][Stopped]");
        }
    }
}