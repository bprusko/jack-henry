using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoSmart.Unicode;
using Interview.Models.Tweets;
using Interview.Utilities;

namespace Interview.Tests.Utilities
{
    [TestClass]
    public class TweetsFilterTests
    {
        #region EmojiList

        [TestMethod]
        public void EmojiListIsDefined()
        {
            Assert.IsNotNull(TweetsFilter.EmojiList);
            Assert.AreEqual(2619, TweetsFilter.EmojiList.Count);
        }

        #endregion

        #region FindEmoji

        [TestMethod]
        public void FindEmoji_WhenTweetDoesNotHaveAnyEmoji()
        {
            var tweet = new Tweet
            {
                Data = new TweetData
                {
                    Text = "Tweet text"
                }
            };
            Assert.AreEqual(0, TweetsFilter.FindEmoji(tweet).Count);
        }

        [TestMethod]
        public void FindEmoji_WhenATweetHasSingleEmoj()
        {
            var tweet = new Tweet
            {
                Data = new TweetData
                {
                    Text = "Happy Birthday, Melissa!🎂"
                }
            };

            Assert.AreEqual(1, TweetsFilter.FindEmoji(tweet).Count);
        }

        [TestMethod]
        public void FindEmoji_WhenATweetHasDifferentEmoji()
        {
            Assert.AreEqual(5, TweetsFilter.FindEmoji(TestData.TweetWithDifferentEmoji).Count);
        }

        [TestMethod]
        public void FindEmoji_WhenATweetsHaveMultipleInstancesOfTheSameEmojSeparatedBySpaces()
        {
            var tweet = new Tweet
            {
                Data = new TweetData
                {
                    Text = "@estarcevic_ Congrats! 👏 👏 👏"
                }
            };

            Assert.AreEqual(1, TweetsFilter.FindEmoji(tweet).Count);
        }

        [TestMethod]
        public void FindEmoji_WhenATweetsHaveConsecutiveInstancesOfTheSameEmojNotSeparatedBySpaces()
        {
            var tweet = new Tweet
            {
                Data = new TweetData
                {
                    Text = "@estarcevic_ Congrats! 👏👏👏"
                }
            };

            Assert.AreEqual(1, TweetsFilter.FindEmoji(tweet).Count);
        }

        #endregion

        #region GetTopDomains

        [TestMethod]
        public void GetTopDomains_ReturnsTopThreeDomains()
        {
            const string Url1 = "https://www.twitter.com/ABC";
            const string Url2 = "https://www.wwe.com/DEF";
            const string Url3 = "https://www.cnn.com/GHI";

            var urls = new List<string> {
                Url1,
                Url1,
                Url2,
                Url1,
                Url2,
                Url3,
                Url3,
                "https://not.twitter.com/GHI"
            };

            var tweets = new List<Tweet> {
                new Tweet {
                    Data = new TweetData {
                        Entities = new TweetEntities {
                            Urls = new Url[] {
                                new Url {ExpandedUrl=Url1},
                            }
                        }
                    }
                },
                new Tweet {
                    Data = new TweetData {
                        Entities = new TweetEntities {
                            Urls = new Url[] {
                                new Url {ExpandedUrl=Url1},
                                new Url {ExpandedUrl=Url2}
                            }
                        }
                    }
                },
                new Tweet {
                    Data = new TweetData {
                        Entities = new TweetEntities {
                            Urls = new Url[] {
                                new Url {ExpandedUrl=Url1},
                                new Url {ExpandedUrl=Url2},
                                new Url {ExpandedUrl=Url3}
                            }
                        }
                    }
                },
                new Tweet {
                    Data = new TweetData {
                        Entities = new TweetEntities {
                            Urls = new Url[] {
                                new Url {ExpandedUrl=Url1},
                                new Url {ExpandedUrl=Url2},
                                new Url {ExpandedUrl=Url3},
                                new Url {ExpandedUrl="https://not.twitter.com/GHI"}
                            }
                        }
                    }
                },
            };

            var hosts = TweetsFilter.GetTopDomains(urls);
            Assert.AreEqual(3, hosts.Count());
            Assert.AreEqual("www.twitter.com", hosts[0]);
            Assert.AreEqual("www.wwe.com", hosts[1]);
            Assert.AreEqual("www.cnn.com", hosts[2]);
        }

        [TestMethod]
        public void GetTopDomains_ReturnsEmptyList_WhenTweetsHaveNoUrls()
        {
            var hosts = TweetsFilter.GetTopDomains(new List<string>());
            Assert.AreEqual(0, hosts.Count());
        }

        #endregion

        #region GetTopEmoji

        [TestMethod]
        public void GetTopEmoji_ReturnsTopThreeEmoji()
        {
            var birthdayCake = Emoji.BirthdayCake.ToString();
            var clappingHands = Emoji.ClappingHands.ToString();
            var americanFootball = Emoji.AmericanFootball.ToString();
            var boxingGlove = Emoji.BoxingGlove.ToString();

            var emojiList = new List<string> {
                clappingHands,
                clappingHands,
                birthdayCake,
                clappingHands,
                americanFootball,
                birthdayCake,
                clappingHands,
                birthdayCake,
                americanFootball,
                boxingGlove
            };

            var topEmoji = TweetsFilter.GetTopEmoji(emojiList);
            Assert.AreEqual(3, topEmoji.Count());
            Assert.AreEqual(clappingHands, topEmoji[0]);
            Assert.AreEqual(birthdayCake, topEmoji[1]);
            Assert.AreEqual(americanFootball, topEmoji[2]);
        }

        [TestMethod]
        public void GetTopEmoji_ReturnsEmptyList_WhenThereAreNoTweetsWithEmoji()
        {
            var emoji = TweetsFilter.GetTopEmoji(new List<string>());
            Assert.AreEqual(0, emoji.Count());
        }

        #endregion

        #region GetTopHashtags

        [TestMethod]
        public void GetTopHashtags_ReturnsTopThreeHashtags()
        {
            var hashtags = new List<string>{
                "Hashtag1",
                "Hashtag1",
                "Hashtag2",
                "Hashtag1",
                "Hashtag2",
                "Hashtag3",
                "Hashtag3",
                "Hashtag4"
            };

            var topHashtags = TweetsFilter.GetTopHashtags(hashtags);
            Assert.AreEqual(3, topHashtags.Count());
            Assert.AreEqual("Hashtag1", topHashtags[0]);
            Assert.AreEqual("Hashtag2", topHashtags[1]);
            Assert.AreEqual("Hashtag3", topHashtags[2]);
        }

        [TestMethod]
        public void GetTopHashtags_ReturnsEmptyList_WhenTweetsHaveNoUrls()
        {
            var hastags = TweetsFilter.GetTopHashtags(new List<string>());
            Assert.AreEqual(0, hastags.Count());
        }

        #endregion

        #region GetPercentTweetsWithEmojis

        [TestMethod]
        public void GetPercentTweetsWithEmoji_ReturnsZero_WhenThereAreNoTweetsWithEmoji()
        {
            var percentTweetsWithEmoji = TweetsFilter.GetPercentTweetsWithEmoji(new List<Tweet>(), 1);
            Assert.AreEqual(0, percentTweetsWithEmoji);
        }

        [TestMethod]
        public void GetPercentTweetsWithEmoji_ReturnsPercentage_WhenThereTweetsWithEmoji()
        {
            var tweets = new List<Tweet> {
                new Tweet { Data = new TweetData {Text = "Congrats! 👏👏👏"}}
            };

            var percentTweetsWithEmoji = TweetsFilter.GetPercentTweetsWithEmoji(tweets, tweets.Count * 3);
            Assert.AreEqual((decimal)33.33, percentTweetsWithEmoji);
        }

        #endregion

        #region GetPercentTweetsWithPhotoUrls

        [TestMethod]
        public void GetPercentTweetsWithPhotoUrls_ReturnsZero_WhenTweetsHaveNoUrls()
        {
            var percentTweetsWithPhotoUrls = TweetsFilter.GetPercentTweetsWithPhotoUrls(new List<Tweet>(), 1);
            Assert.AreEqual(0, percentTweetsWithPhotoUrls);
        }

        [TestMethod]
        public void GetPercentTweetsWithPhotoUrls_ReturnsPercentage_WhenSomeTweetsHavePhotoUrls()
        {
            var tweets = new List<Tweet> {
                new Tweet {
                    Data = new TweetData {
                        Entities = new TweetEntities {
                            Urls = new Url[] {
                                new Url {DisplayUrl="pic.twitter.com/QNCBu0LkFx"},
                                new Url {DisplayUrl="not.twitter.com/ABC"}
                            }
                        }
                    }
                }
            };

            var percentTweetsWithPhotoUrls = TweetsFilter.GetPercentTweetsWithPhotoUrls(tweets, tweets.Count * 3);
            Assert.AreEqual((decimal)33.33, percentTweetsWithPhotoUrls);
        }

        [TestMethod]
        public void GetPercentTweetsWithPhotoUrls_ReturnsOneHundred_WhenAllTweetsHavePhotoUrls()
        {
            var tweets = new List<Tweet> {
                new Tweet {
                    Data = new TweetData {
                        Entities = new TweetEntities {
                            Urls = new Url[] {
                                new Url {DisplayUrl="pic.twitter.com/QNCBu0LkFx"},
                            }
                        }
                    }
                },
                new Tweet {
                    Data = new TweetData {
                        Entities = new TweetEntities {
                            Urls = new Url[] {
                                new Url {DisplayUrl="pic.twitter.com/QNCBu0LkFx2"},
                            }
                        }
                    }
                },
            };
            Assert.AreEqual(100, TweetsFilter.GetPercentTweetsWithPhotoUrls(tweets, tweets.Count));
        }

        #endregion

        #region GetPercentTweetsWithUrls

        [TestMethod]
        public void GetPercentTweetsWithUrls_ReturnsZero_WhenTweetsHaveNoUrls()
        {
            var percentTweetsWithUrls = TweetsFilter.GetPercentTweetsWithUrls(new List<Tweet>(), 5);
            Assert.AreEqual(0, percentTweetsWithUrls);
        }

        [TestMethod]
        public void GetPercentTweetsWithUrls_ReturnsPercentage_WhenSomeTweetsHaveUrls()
        {
            var tweets = new List<Tweet> {
                new Tweet {
                    Data = new TweetData {
                        Entities = new TweetEntities {
                            Urls = new Url[] {
                                new Url {DisplayUrl="pic.twitter.com/QNCBu0LkFx"},
                                new Url {DisplayUrl="not.twitter.com/ABC"}
                            }
                        }
                    }
                }
            };

            Assert.AreEqual((decimal)33.33, TweetsFilter.GetPercentTweetsWithUrls(tweets, tweets.Count * 3));
        }

        [TestMethod]
        public void GetPercentTweetsWithUrls_ReturnsOneHundred_WhenAllTweetsHaveUrls()
        {
            var tweets = new List<Tweet> {
                new Tweet {
                    Data = new TweetData {
                        Entities = new TweetEntities {
                            Urls = new Url[] {
                                new Url {DisplayUrl="pic.twitter.com/QNCBu0LkFx"},
                                new Url {DisplayUrl="not.twitter.com/ABC"}
                            }
                        }
                    }
                },
                new Tweet {
                    Data = new TweetData {
                        Entities = new TweetEntities {
                            Urls = new Url[] {
                                new Url {DisplayUrl="pic.twitter.com/QNCBu0LkFx"},
                                new Url {DisplayUrl="not.twitter.com/ABC"}
                            }
                        }
                    }
                },
            };
            Assert.AreEqual(100, TweetsFilter.GetPercentTweetsWithUrls(tweets, tweets.Count));
        }

        #endregion
    }
}