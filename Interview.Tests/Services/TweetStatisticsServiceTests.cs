using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Interview.Models.Tweets;
using Interview.Services;
using Interview.Services.Interfaces;

namespace Interview.Tests.Services
{
    [TestClass]
    public class TweetStatisticsServiceTests
    {
        [TestMethod]
        public void WhenThereAreNoTweets_AllStatisticsAreDefaultValues()
        {
            var statsService = new TweetStatisticsService();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(0, stats.AvgTweetRates.AvgTweetsPerSecond);
            Assert.AreEqual(0, stats.AvgTweetRates.AvgTweetsPerMinute);
            Assert.AreEqual(0, stats.AvgTweetRates.AvgTweetsPerHour);
            Assert.AreEqual(0, stats.TopDomains.Count);
            Assert.AreEqual(0, stats.TopEmoji.Count);
            Assert.AreEqual(0, stats.TopHashtags.Count);
            Assert.AreEqual(0, stats.Total);
            Assert.AreEqual(0, stats.PercentWithEmoji);
            Assert.AreEqual(0, stats.PercentWithPhotoUrls);
            Assert.AreEqual(0, stats.PercentWithUrls);
        }

        #region Average Tweet rates

        [TestMethod]
        public async Task AvgTweetRates_TweetsPerSecond_IsGreaterThanZero()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetList);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.IsTrue(stats.AvgTweetRates.AvgTweetsPerSecond > 0);
        }

        #endregion

        #region Percentage with Emoji
        [TestMethod]
        public async Task PercentageWithEmojis_IsZero_WhenNoTweetsHaveEmoji()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetList);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(0, stats.PercentWithEmoji);
        }

        [TestMethod]
        public async Task PercentageWithEmojis_IsGreaterThanZero_WhenSomeTweetsHaveEmoji()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetListWithSomeEmoji);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual((decimal)33.33, stats.PercentWithEmoji);
        }

        #endregion

        #region Percentage With Photo Urls
        [TestMethod]
        public async Task PercentageWithPhotoUrls_IsGreaterThanZero_WhenSomeTweetsHavePhotoUrls()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetList);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual((decimal)75.00, stats.PercentWithPhotoUrls);
        }

        [TestMethod]
        public async Task PercentageWithPhotoUrls_IsOneHundred_WhenAllTweetsHavePhotoUrls()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetListWithAllEntities);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual((decimal)100.00, stats.PercentWithPhotoUrls);
        }

        [TestMethod]
        public async Task PercentageWithPhotoUrls_IsZero_WhenNoTweetsHaveAnyPhotoUrls()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetListWithoutEntities);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(0, stats.PercentWithPhotoUrls);
        }

        #endregion

        #region Percent With Urls
        [TestMethod]
        public async Task PercentWithUrls_IsGreaterThanZero_WhenSomeTweetsHaveUrls()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetList);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual((decimal)75.00, stats.PercentWithUrls);
        }

        [TestMethod]
        public async Task PercentWithUrls_IsOneHundred_WhenAllTweetsHaveUrls()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetListWithAllEntities);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual((decimal)100.00, stats.PercentWithUrls);
        }

        [TestMethod]
        public async Task PercentWithUrls_IsZero_WhenTweetsHaveNoUrls()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetListWithoutEntities);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(0, stats.PercentWithUrls);
        }

        #endregion

        #region Top Domains

        [TestMethod]
        public async Task TopDomains_HasMaxThreeItems_WhenThereAreMoreThanThreeUrls()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetList);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(3, stats.TopDomains.Count);
            Assert.AreEqual("www.twitter.com", stats.TopDomains[0]);
            Assert.AreEqual("www.wwe.com", stats.TopDomains[1]);
            Assert.AreEqual("www.cnn.com", stats.TopDomains[2]);
        }

        [TestMethod]
        public async Task TopDomains_IsEmpty_WhenNoTweetsHaveAnyUrls()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetListWithoutEntities);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(0, stats.TopDomains.Count);
        }

        [TestMethod]
        public async Task TopDomains_HasFewerThanThreeItems_WhenThereAreFewerThanThreeUrls()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetList.Take(2).ToList());
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(2, stats.TopDomains.Count);
            Assert.AreEqual("www.twitter.com", stats.TopDomains[0]);
            Assert.AreEqual("www.wwe.com", stats.TopDomains[1]);
        }

        #endregion

        #region Top Emoji
        [TestMethod]
        public async Task TopEmoji_IsEmpty_WhenNoTweetsHaveEmoji()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetList);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(0, stats.TopEmoji.Count);
        }

        [TestMethod]
        public async Task TopEmoji_HasMaxThreeItems_WhenThereAreMoreThanThreeEmoji()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, new List<Tweet>{ TestData.TweetWithDifferentEmoji });
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(3, stats.TopEmoji.Count);
        }

        [TestMethod]
        public async Task TopEmoji_HasFewerThanThreeItems_WhenThereAreFewerThanThreeEmoji()
        {
            var tweetList = new List<Tweet> {
                new Tweet {
                    Data = new TweetData {
                        Text = "Tweet with 2 emoji 👍 📺"
                    }
                }
            };

            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, tweetList);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(2, stats.TopEmoji.Count);
        }

        #endregion

        #region Top Hashtags
        [TestMethod]
        public async Task TopHashtags_HasMaxThreeItems_WhenThereAreMoreThanThreeHashtags()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetList);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(3, stats.TopHashtags.Count);
            Assert.AreEqual("Hashtag1", stats.TopHashtags[0]);
            Assert.AreEqual("Hashtag2", stats.TopHashtags[1]);
            Assert.AreEqual("Hashtag3", stats.TopHashtags[2]);
        }

        [TestMethod]
        public async Task TopHashtags_IsEmpty_WhenNoTweetsHaveAnyHastags()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetListWithoutEntities);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(0, stats.TopHashtags.Count);
        }

        [TestMethod]
        public async Task TopHashtags_HasFewerThanThreeItems_WhenThereAreFewerThanThreeHashtags()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetList.Take(2).ToList());
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(2, stats.TopHashtags.Count);
            Assert.AreEqual("Hashtag1", stats.TopHashtags[0]);
            Assert.AreEqual("Hashtag2", stats.TopHashtags[1]);
        }
        #endregion

        [TestMethod]
        public async Task TotalTweets()
        {
            var statsService = new TweetStatisticsService();
            SetUpTweetList(statsService, TestData.TweetList);
            await statsService.UpdateStatisticsAsync();
            var stats = statsService.GetStatistics();
            Assert.AreEqual(TestData.TweetList.Count, stats.Total);
        }

        private void SetUpTweetList(ITweetStatisticsService statisticsService, List<Tweet> tweets){
            foreach(var tweet in tweets) {
                statisticsService.AddTweet(tweet);
            }
        }
    }
}