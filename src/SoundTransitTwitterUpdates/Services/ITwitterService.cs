using System;
using System.Collections.Generic;
using RestSharp;
using SoundTransitTwitterUpdates.Responses;

namespace SoundTransitTwitterUpdates.Services
{
    public interface ITwitterService
    {
        IRestClient Client { get; set; }

        IEnumerable<ITwitterResponse> GetRecentTweets(int numberOfHours);
    }
}
