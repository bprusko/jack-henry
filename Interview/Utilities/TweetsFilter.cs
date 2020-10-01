using System;
using System.Collections.Generic;
using System.Linq;
using Interview.Models.Tweets;
using Interview.Models.Statistics;
using NeoSmart.Unicode;

namespace Interview.Utilities
{

    public static class TweetsFilter
    {
        /* As of September 2020, there are 3,521 emoji in the Unicode Standard.
           The Unicode.net project covers 2,619 emoji, so this list isn't current,
           but it seems more complete than the Emoji Data project,
           which has 1,743 emoji in its emoji JSON.
        */
        public static readonly List<string> EmojiList =  Emoji.All.Select(x=> x.ToString()).ToList();

        public static List<string> FindEmoji(Tweet tweet) {
            return EmojiList.Intersect(tweet?.Data?.Text?.Letters()).ToList();
        }

        public static TweetRates GetAverageTweetRates(DateTime updatesStartedAt, int totalTweets)
        {
            var currentTime = DateTime.Now;
            var elapsedTimeInSeconds = currentTime.Subtract(updatesStartedAt).TotalSeconds;
            var elapsedTimeInMinutes = currentTime.Subtract(updatesStartedAt).TotalMinutes;
            var elapsedTimeInHours = currentTime.Subtract(updatesStartedAt).TotalHours;

            return new TweetRates
            {
                AvgTweetsPerSecond = Math.Round(totalTweets / elapsedTimeInSeconds, 2),
                AvgTweetsPerMinute = elapsedTimeInMinutes >= 1 ? Math.Round(totalTweets / elapsedTimeInMinutes, 2) : 0,
                AvgTweetsPerHour = elapsedTimeInHours >= 1 ? Math.Round(totalTweets / elapsedTimeInHours, 2) : 0
            };
        }

        public static decimal GetPercentTweetsWithEmoji(List<Tweet> tweetsWithEmoji, int totalTweets)
        {
            return Math.Round(((decimal)tweetsWithEmoji.Count / totalTweets) * 100, 2);
        }

        public static decimal GetPercentTweetsWithUrls(List<Tweet> tweetsWithUrls, int totalTweets)
        {
            return Math.Round(((decimal)tweetsWithUrls.Count / totalTweets) * 100, 2);
        }

        public static decimal GetPercentTweetsWithPhotoUrls(List<Tweet> tweetsWithPhotoUrls, int totalTweets)
        {
            return Math.Round(((decimal)tweetsWithPhotoUrls.Count / totalTweets) * 100, 2);
        }

        public static List<string> GetTopDomains(List<string> urls)
        {
            if (urls.Count > 0)
            {
                /* We need to copy the list, because
                   we could encounter the following exception if the list in the paramaters is modified
                   while we're trying to enumerate through it:
                   Collection was modified; enumeration operation may not execute
                */
                var domains = urls.Select(x => x).ToList();
                var uris = domains.Select(u => new Uri(u))
                                         .ToList()
                                         .Select(x => x.Host)
                                         .ToList();

                return uris.GroupBy(d => d)
                                      .OrderByDescending(d => d.Count())
                                      .Take(3)
                                      .Select(d => d.Key)
                                      .ToList();
            }

            return new List<string>();
        }

        public static List<string> GetTopEmoji(List<string> emoji)
        {
            if (emoji.Count > 0)
            {
                /* We need to copy the list, because
                   we could encounter the following exception if the list in the paramaters is modified
                   while we're trying to enumerate through it:
                   Collection was modified; enumeration operation may not execute
                */

                var topEmoji = emoji.Select(x => x).ToList();

                return topEmoji.GroupBy(h => h)
                                             .OrderByDescending(ht => ht.Count())
                                             .Take(3)
                                             .Select(x => x.Key)
                                             .ToList();

                // If we want to return top hashtags with number of occurrences for each
                /* return tags.GroupBy(h => h)
                                        .OrderByDescending(ht => ht.Count())
                                        .Select(x => new {x.Key, Count = x.Count() })
                                        .Take(3)
                                        .ToDictionary(y => y.Key, y => y.Count); */
            }

            return new List<string>();
        }

        public static List<string> GetTopHashtags(List<string> hashtags)
        {
            if (hashtags.Count > 0)
            {
                /* We need to copy the list, because
                   we could encounter the following exception if the list in the paramaters is modified
                   while we're trying to enumerate through it:
                   Collection was modified; enumeration operation may not execute
                */

                var tags = hashtags.Select(x => x).ToList();
                return tags.GroupBy(h => h)
                                             .OrderByDescending(ht => ht.Count())
                                             .Take(3)
                                             .Select(x => x.Key)
                                             .ToList();

                // If we want to return top hashtags with number of occurrences for each
                /* return tags.GroupBy(h => h)
                                        .OrderByDescending(ht => ht.Count())
                                        .Select(x => new {x.Key, Count = x.Count() })
                                        .Take(3)
                                        .ToDictionary(y => y.Key, y => y.Count); */
            }

            return new List<string>();
        }
    }
}