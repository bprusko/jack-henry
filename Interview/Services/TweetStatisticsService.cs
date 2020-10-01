using System.Linq;
using System.Threading.Tasks;
using System;
using Interview.Models.Statistics;
using Interview.Models.Tweets;
using Interview.Utilities;
using System.Collections.Generic;
using Interview.Services.Interfaces;

namespace Interview.Services
{
    public class TweetStatisticsService : ITweetStatisticsService
    {
        #region Statistics
        private TweetRates AvgTweetRates;

        private decimal PercentTweetsWithEmoji;

        private decimal PercentTweetsWithPhotoUrls;

        private decimal PercentTweetsWithUrls;

        private List<string> TopDomains;

        private List<string> TopEmoji;

        private List<string> TopHashtags;

        private int Total;

        private List<Tweet> Tweets { get; set; }

        #endregion

        #region Aggregate Lists

        /* Maintain separate lists so we don't need to filter
           the entire list of tweets received each time we want
           to calculate statistics for emoji, hashtags, and URLs.
        */

        private List<string> AllEmoji;

        private List<string> AllHashtags;

        private List<string> AllUrls;

        private List<Tweet> TweetsWithEmoji { get; set; }

        private List<Tweet> TweetsWithPhotoUrls { get; set; }

        private List<Tweet> TweetsWithUrls { get; set; }

        #endregion

        private DateTime UpdatesStartedAt;

        public TweetStatisticsService()
        {
            AllEmoji = new List<string>();
            AllHashtags = new List<string>();
            AllUrls = new List<string>();
            AvgTweetRates = new TweetRates();
            TopDomains = new List<string>();
            TopEmoji = new List<string>();
            TopHashtags = new List<string>();
            Tweets = new List<Tweet>();
            TweetsWithEmoji = new List<Tweet>();
            TweetsWithPhotoUrls = new List<Tweet>();
            TweetsWithUrls = new List<Tweet>();
            UpdatesStartedAt = DateTime.Now;
        }

        public void AddTweet(Tweet tweet)
        {
            Parallel.Invoke(
                () => {
                    Tweets.Add(tweet);
                },
                () => {
                    UpdateEmojiLists(tweet);
                },
                () => {
                    UpdateHashtags(tweet);
                },
                () => {
                    UpdateUrlLists(tweet);
                }
            );
        }

        private void UpdateEmojiLists(Tweet tweet)
        {
            try
            {
                var tweetText = tweet?.Data?.Text;

                if (!string.IsNullOrEmpty(tweetText))
                {
                    var emoji = TweetsFilter.FindEmoji(tweet);
                    if (emoji.Count > 0)
                    {
                        TweetsWithEmoji.Add(tweet);
                        AllEmoji.AddRange(emoji);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[TWEET STATISTICS SERVICE][EXCPETION][UpdateEmojiLists]");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void UpdateHashtags(Tweet tweet)
        {
            try
            {
                var hashtags = tweet?.Data?.Entities?.Hashtags;

                if (hashtags?.Count() > 0)
                {
                    var allHashtags = hashtags
                                        .Select(t => t.Tag)
                                        .ToList();

                    AllHashtags.AddRange(allHashtags);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[TWEET STATISTICS SERVICE][EXCPETION][UpdateHashtags]");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void UpdateUrlLists(Tweet tweet)
        {
            try
            {
                var urls = tweet?.Data?.Entities?.Urls;

                if (urls?.Count() > 0)
                {
                    TweetsWithUrls.Add(tweet);

                    /* I'm using the ExpandedUrl property here,
                       because the Display URL doesn't work with my approach
                       for generating the top domains.
                    */
                    var allUrls = urls.Select(u => u.ExpandedUrl).ToList();

                    AllUrls.AddRange(allUrls);

                    if (tweet.Data.Entities.HasPhotoUrl())
                    {
                        TweetsWithPhotoUrls.Add(tweet);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[TWEET STATISTICS SERVICE][EXCPETION][UpdateUrlLists]");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public TweetStatistics GetStatistics()
        {
            return new TweetStatistics
            {
                AvgTweetRates = AvgTweetRates,
                PercentWithEmoji = PercentTweetsWithEmoji,
                PercentWithPhotoUrls = PercentTweetsWithPhotoUrls,
                PercentWithUrls = PercentTweetsWithUrls,
                TopDomains = TopDomains,
                TopEmoji = TopEmoji,
                TopHashtags = TopHashtags,
                Total = Total
            };
        }

        public async Task UpdateStatisticsAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    Total = Tweets.Count;

                    if (Total > 0)
                    {
                        AvgTweetRates = TweetsFilter.GetAverageTweetRates(UpdatesStartedAt, Total);
                        PercentTweetsWithEmoji = TweetsFilter.GetPercentTweetsWithEmoji(TweetsWithEmoji, Total);
                        PercentTweetsWithPhotoUrls = TweetsFilter.GetPercentTweetsWithPhotoUrls(TweetsWithPhotoUrls, Total);
                        PercentTweetsWithUrls = TweetsFilter.GetPercentTweetsWithUrls(TweetsWithUrls, Total);
                        TopDomains = TweetsFilter.GetTopDomains(AllUrls);
                        TopEmoji = TweetsFilter.GetTopEmoji(AllEmoji);
                        TopHashtags = TweetsFilter.GetTopHashtags(AllHashtags);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("[TWEET STATISTICS SERVICE][EXCPETION][UpdateStatisticsAsync]");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}