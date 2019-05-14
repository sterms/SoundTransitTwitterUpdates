using System;
using System.Collections.Generic;
using RestSharp;
using SoundTransitTwitterUpdates.Responses;

namespace SoundTransitTwitterUpdates.Services
{
    public class TwitterServiceStub : ITwitterService
    {
        public IRestClient Client { get; set; }

        public IEnumerable<ITwitterResponse> GetRecentTweets(int numberOfHours)
        {
            return new List<TwitterResponse>()
            {
                new TwitterResponse()
                {
                    CreatedAt = DateTimeOffset.Now.AddHours(-1),
                    UID = "1",
                    FullText = "Test Message 1",
                    Truncated = false
                },
                new TwitterResponse()
                {
                    CreatedAt = DateTimeOffset.Now.AddHours(-2),
                    UID = "2",
                    FullText = "Test Message 2",
                    Truncated = false
                },
                new TwitterResponse()
                {
                    CreatedAt = DateTimeOffset.Now.AddHours(-3),
                    UID = "3",
                    FullText = "Test Message 3",
                    Truncated = false
                },
                new TwitterResponse()
                {
                    CreatedAt = DateTimeOffset.Now.AddHours(-4),
                    UID = "4",
                    FullText = "Test Message 4",
                    Truncated = false
                },
                new TwitterResponse()
                {
                    CreatedAt = DateTimeOffset.Now.AddHours(-5),
                    UID = "5",
                    FullText = "Test Message 5",
                    Truncated = false
                },
                new TwitterResponse()
                {
                    CreatedAt = DateTimeOffset.Now.AddHours(-6),
                    UID = "6",
                    FullText = "Test Message 6",
                    Truncated = false
                }
            };
        }
    }
}
