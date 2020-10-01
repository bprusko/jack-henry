using Interview.Models.Statistics;
using Interview.Models.Tweets;
using System.Threading.Tasks;

namespace Interview.Services.Interfaces
{
    public interface ITweetStatisticsService
    {
        void AddTweet(Tweet tweet);

        TweetStatistics GetStatistics();

        Task UpdateStatisticsAsync();
    }
}