using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interview.Models.Tweets;

namespace Interview.Tests.Models
{
    [TestClass]
    public class TweetEntitiesTests
    {
        [TestMethod]
        public void HasPhotoUrl_ReturnsTrue_WhenUrlsContainsAtLeastOneTwitterPicUrl()
        {
            var tweet = new TweetEntities{
                Urls = new Url[] {
                    new Url {DisplayUrl="pic.twitter.com/QNCBu0LkFx"},
                    new Url {DisplayUrl="not.twitter.com/ABC"}
                }
            };
            Assert.IsTrue(tweet.HasPhotoUrl());
        }

        [TestMethod]
        public void HasPhotoUrl_ReturnsTrue_WhenUrlsContainsAtLeastOneInstagramPicUrl()
        {
            var tweet = new TweetEntities{
                Urls = new Url[] {
                    new Url {DisplayUrl="instagram.com/QNCBu0LkFx"},
                    new Url {DisplayUrl="not.twitter.com/ABC"}
                }
            };
            Assert.IsTrue(tweet.HasPhotoUrl());
        }

        [TestMethod]
        public void HasPhotoUrl_ReturnsFalse_WhenThereAreNoPhotoUrls()
        {
            var tweet = new TweetEntities{
                Urls = new Url[] {
                    new Url {DisplayUrl="pc.twitter.com/QNCBu0LkFx"},
                    new Url {DisplayUrl="pc.twitter.com/ABC"}
                }
            };
            Assert.IsFalse(tweet.HasPhotoUrl());
        }

        [TestMethod]
        public void HasPhotoUrl_ReturnsFalse_WhenThereAreNoUrls()
        {
            var tweet = new TweetEntities();
            Assert.IsFalse(tweet.HasPhotoUrl());
        }
    }
}
