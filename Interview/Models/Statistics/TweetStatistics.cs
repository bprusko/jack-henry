using Newtonsoft.Json;
using System.Collections.Generic;

namespace Interview.Models.Statistics
{
    public class TweetStatistics
    {
        [JsonProperty("AvgTweetRates")]
        public TweetRates AvgTweetRates {get; set;}

        [JsonProperty("PercentWithEmoji")]
        public decimal PercentWithEmoji {get; set;}

        [JsonProperty("PercentWithPhotoUrls")]
        public decimal PercentWithPhotoUrls {get; set;}

        [JsonProperty("PercentWithUrls")]
        public decimal PercentWithUrls {get; set;}

        [JsonProperty("TopDomains")]
        public List<string> TopDomains {get; set;}

        [JsonProperty("TopEmoji")]
        public List<string> TopEmoji {get; set;}

        [JsonProperty("TopHashtags")]
        public List<string> TopHashtags {get; set;}

        [JsonProperty("Total")]
        public int Total {get; set;}
    }
}