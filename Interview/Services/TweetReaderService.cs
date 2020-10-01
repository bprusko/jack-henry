using System.Net;
using System.IO;
using Interview.Models.Tweets;
using Interview.Services.Interfaces;
using Newtonsoft.Json;

namespace Interview.Services
{
    public class TweetReaderService : ITweetReaderService
    {
        private ITweetStatisticsService _tweetStatisticsService;

        public TweetReaderService(ITweetStatisticsService tweetStatisticsService) {
            _tweetStatisticsService = tweetStatisticsService;
        }

        public void ReadStream()
        {
            WebRequest request = WebRequest.Create("https://api.twitter.com/2/tweets/sample/stream?tweet.fields=entities");
            request.Headers.Add("Authorization", "Bearer $HEY_GET_YOUR_OWN_TOKEN");

            request.BeginGetResponse(ar =>
            {
                var req = (WebRequest)ar.AsyncState;
                using (var tweets = req.EndGetResponse(ar))
                using (var reader = new StreamReader(tweets.GetResponseStream()))
                {
                    while (!reader.EndOfStream)
                    {
                        var rawTweet = reader.ReadLine();
                        var tweet = JsonConvert.DeserializeObject<Tweet>(rawTweet);
                        _tweetStatisticsService.AddTweet(tweet);
                    }
                }
            }, request);
        }

    }
}