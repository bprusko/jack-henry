using Newtonsoft.Json;

namespace Interview.Models.Tweets
{
    public class TweetData
    {
        [JsonProperty("entities")]
        public TweetEntities Entities {get; set;}

        [JsonProperty("text")]
        public string Text {get; set;}
    }
}