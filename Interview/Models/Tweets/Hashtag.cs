using Newtonsoft.Json;

namespace Interview.Models.Tweets
{
    public class Hashtag
    {
        [JsonProperty("tag")]
        public string Tag {get; set;}
    }
}