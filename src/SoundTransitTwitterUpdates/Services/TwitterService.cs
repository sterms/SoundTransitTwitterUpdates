using System;
using System.Collections.Generic;
using System.Linq;
using SoundTransitTwitterUpdates.Responses;
using SoundTransitTwitterUpdates.Utilities;
using Amazon.Lambda.Core;
using RestSharp;

namespace SoundTransitTwitterUpdates.Services
{
    public class TwitterService : ITwitterService
    {
        public IRestClient Client { get; set; }

        private ITokenService TokenService { get; set; }

        public TwitterService(ITokenService tokenService)
        {
            TokenService = tokenService;
            Client = new RestClient("https://api.twitter.com/1.1/statuses/user_timeline.json");
            Client.AddHandler("application/json", new LambdaRestSerializer());
        }

        public IEnumerable<ITwitterResponse> GetRecentTweets(int numberOfHours)
        {
            IEnumerable<ITwitterResponse> responses = null;
            IEnumerable<ITwitterResponse> filterResponses = null;
            int tweetsToPull = 0;
            int availableCalls = 4;

            while (availableCalls >= 0 && ((responses == null && filterResponses == null) || (responses.Count() == filterResponses.Count())))
            {
                tweetsToPull += 5;
                responses = GetTweets(tweetsToPull);
                availableCalls--;
                filterResponses = responses.Where(x => x.CreatedAt > DateTimeOffset.Now.AddHours(-numberOfHours));
            }

            return filterResponses;
        }

        private IEnumerable<ITwitterResponse> GetTweets(int number = 5)
        {
            var request = new RestRequest(Method.GET)
                .AddQueryParameter("screen_name", "SoundTransit")
                .AddQueryParameter("count", number.ToString())
                .AddQueryParameter("tweet_mode", "extended")
                .AddQueryParameter("include_rts", "false")
                .AddQueryParameter("exclude_replies", "true")
                .AddHeader("Content-type", "application/json")
                .AddHeader("Authorization", TokenService.Token);

            var response = Client.Execute<List<TwitterResponse>>(request);
            if (response.IsSuccessful)
                return response.Data;
            else
                throw new ArgumentException("Service returned error code " + response.StatusCode);
        }
    }
}
