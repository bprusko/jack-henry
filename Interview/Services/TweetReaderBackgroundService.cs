using System.Threading;
using System;
using Interview.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Interview.Services
{
    public class TweetReaderBackgroundService : BackgroundService
    {
        private ITweetReaderService _tweetReaderService;
        public TweetReaderBackgroundService(ITweetReaderService tweetReaderService) {
            _tweetReaderService = tweetReaderService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("[TWEET READER BACKGROUND SERVICE]: Starting to read Tweet stream!");

            _tweetReaderService.ReadStream();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() => {
                   // Console.WriteLine("[TWEET READER BACKGROUND SERVICE]: Reading Tweet stream... ");
                });
            }

            Console.WriteLine("[TWEET READER BACKGROUND SERVICE][Stopped]");
        }
    }
}