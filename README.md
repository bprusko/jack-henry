# Project Notes

This project consumes Twitter's V2 stream API (https://api.twitter.com/2/tweets/sample/stream) and calculates statistics as Tweets arrive.

## Implementation

I built this project in .NET Core 3.1.

One .NET Background Service starts reading the stream, and another Background Service processes the statistics, which are available at http://localhost:5000/api/v1/tweets/statistics.

 When a Tweet comes in, I update aggregate lists for emoji, hashtags, and URLs. I chose this approach so I didn't need to process the entire list of all Tweets received each time I calculate each statistic outlined below.

### Statistics

I calculate the following statistics:

1. __Top domains in URLs included in Tweets__. I've based this on the Expanded URL property that Twitter returns.
1. Top emoji used in Tweets
1. Top hashtags
1. Percent Tweets with emoji
1. Percent Tweets with URLs
1. __Percent Tweets with Photo URLs (pic.twitter.com and instagram.com)__. I've based this on the Display URL property that Twitter returns.
1. Rates for Tweets processed per second/minute/hour. All rates are based on the elapsed time that the service has been running. Tweets per second is available immediately, but I start returning tweets per minute after 1 min has elapsed and tweets per hour after 1 hour has elapsed.
1. Total Tweets

As of September 2020, there are 3,521 emoji in the Unicode Standard. I'm using version 0.1.2 of the [Unicode.net NuGet package](https://github.com/neosmart/unicode.net) to help me process the emoji statistics. This package version includes 2,619 emoji, so this list isn't current, but it is more complete than the Emoji Data project, which was suggested in the exercise instructions. The Emoji Data project emoji JSON has 1,743 emoji at the time of my submission.

## Running the Project

You will need to use your own Bearer token from Twitter in __TweetReaderService.cs__ to call Twitter's API.