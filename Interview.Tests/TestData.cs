using System.Collections.Generic;
using Interview.Models.Statistics;
using Interview.Models.Tweets;

namespace Interview.Tests
{
    public class TestData
    {
        const string Hashtag1 = "Hashtag1";
        const string Hashtag2 = "Hashtag2";
        const string Hashtag3 = "Hashtag3";

        const string DisplayUrl1 = "pic.twitter.com/ABC";
        const string DisplayUrl2 = "instragram.com/DEF";
        const string DisplayUrl3 = "not.twitter.com/";

        const string ExtendedUrl1 = "https://www.twitter.com/ABC";
        const string ExtendedUrl2 = "https://www.wwe.com/DEF";
        const string ExtendedUrl3 = "https://www.cnn.com/GHI";

        public static Tweet Tweet1 = new Tweet
        {
            Data = new TweetData
            {
                Entities = new TweetEntities
                {
                    Hashtags = new[] {
                        new Hashtag {Tag = Hashtag1},
                    },
                    Urls = new[] {
                        new Url {DisplayUrl = DisplayUrl1, ExpandedUrl = ExtendedUrl1}
                    }
                },
                Text = "My first tweet"
            }
        };

        public static Tweet Tweet2 = new Tweet
        {
            Data = new TweetData
            {
                Entities = new TweetEntities
                {
                    Hashtags = new[] {
                        new Hashtag {Tag = Hashtag1},
                        new Hashtag {Tag = Hashtag2}
                    },
                    Urls = new[] {
                        new Url {DisplayUrl = DisplayUrl1, ExpandedUrl = ExtendedUrl1},
                        new Url {DisplayUrl = DisplayUrl2, ExpandedUrl = ExtendedUrl2},
                    }
                },
                Text = "My second tweet"
            }
        };

        public static Tweet Tweet3 = new Tweet
        {
            Data = new TweetData
            {
                Entities = new TweetEntities
                {
                    Hashtags = new[] {
                        new Hashtag {Tag = Hashtag1},
                        new Hashtag {Tag = Hashtag2},
                        new Hashtag {Tag = Hashtag3}
                    },
                    Urls = new[] {
                        new Url {DisplayUrl = DisplayUrl1, ExpandedUrl = ExtendedUrl1},
                        new Url {DisplayUrl = DisplayUrl2, ExpandedUrl = ExtendedUrl2},
                        new Url {DisplayUrl = DisplayUrl3, ExpandedUrl = ExtendedUrl3},
                    }
                },
                Text = "My third tweet"
            }
        };

        public static Tweet TweetWithDifferentEmoji = new Tweet
        {
            Data = new TweetData
            {
                Text = "Happy Birthday, Melissa!🎂🎊🎈🎁🎉"
            }
        };

        public static Tweet TweetWithoutEntitites = new Tweet
        {
            Data = new TweetData
            {
                Entities = new TweetEntities(),
                Text = "My fourth tweet"
            }
        };

        public static List<Tweet> TweetList = new List<Tweet> {
            Tweet1,
            Tweet2,
            Tweet3,
            TweetWithoutEntitites
        };

        public static List<Tweet> TweetListWithAllEntities = new List<Tweet> {
            Tweet1,
            Tweet2,
            Tweet3
        };

        public static List<Tweet> TweetListWithoutEntities = new List<Tweet> {
            TweetWithoutEntitites
        };

        public static List<Tweet> TweetListWithSomeEmoji = new List<Tweet> {
            TweetWithDifferentEmoji,
            new Tweet { Data = new TweetData { Text = "Tweet without emoji #1"} },
            new Tweet { Data = new TweetData { Text = "Tweet without emoji #2"} }
        };

        public static TweetStatistics TweetStats = new TweetStatistics
        {
            AvgTweetRates = new TweetRates(),
            PercentWithEmoji = 0,
            PercentWithPhotoUrls = 50,
            PercentWithUrls = 70,
            TopDomains = new List<string> { "www.wwe.com", "www.cnn.com", "www.bridgeclimb.com" },
            TopEmoji = new List<string>(),
            TopHashtags = new List<string> { "JackHenry", "BMW", "Apex" },
            Total = 100
        };
    }
}