using Newtonsoft.Json;

namespace Interview.Models.Tweets
{
    public class Tweet
    {
        [JsonProperty("data")]
        public TweetData Data {get; set;}
    }
}