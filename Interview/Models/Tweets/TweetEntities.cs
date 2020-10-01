using System.Linq;
using Newtonsoft.Json;

namespace Interview.Models.Tweets
{
    public class TweetEntities
    {
        [JsonProperty("hashtags")]
        public Hashtag[] Hashtags { get; set; }

        [JsonProperty("urls")]
        public Url[] Urls { get; set; }

        public bool HasPhotoUrl()
        {
            if(Urls?.Count() > 0 ) {
                var photoUrls = Urls.Where(u => u.DisplayUrl.Contains("pic.twitter")
                                                || u.DisplayUrl.Contains("instagram.com")).ToList();
                return photoUrls.Count > 0;
            }

            return false;
        }
        
    }
}