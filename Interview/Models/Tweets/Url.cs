using Newtonsoft.Json;

namespace Interview.Models.Tweets
{
    public class Url
    {
        [JsonProperty("display_url")]
        public string DisplayUrl {get; set;}

        [JsonProperty("expanded_url")]
        public string ExpandedUrl {get; set;}
    }
}