using Newtonsoft.Json;

namespace Interview.Models.Statistics
{
    public class TweetRates
    {
        [JsonProperty("AvgTweetsPerHour")]
        public double AvgTweetsPerHour {get; set;}

        [JsonProperty("AvgTweetsPerMinute")]
        public double AvgTweetsPerMinute {get; set;}

        [JsonProperty("AvgTweetsPerSecond")]
        public double AvgTweetsPerSecond {get; set;}
    }
}