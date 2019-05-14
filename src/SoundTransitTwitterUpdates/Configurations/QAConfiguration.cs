using System;
using SoundTransitTwitterUpdates.Services;

namespace SoundTransitTwitterUpdates.Configurations
{
    public class QAConfiguration : IConfiguration
    {
        public ITwitterService TwitterService { get; set; }

        public ITwitterService GetTwitterService()
        {
            if (TwitterService == null)
                TwitterService = new TwitterService(GetTokenService());
            return TwitterService;
        }


        public ITokenService TokenService { get; set; } = new TokenService("QA");

        public ITokenService GetTokenService() => TokenService;


        public ISimpleStorageService SimpleStorageService { get; set; }

        public ISimpleStorageService GetSimpleStorageService()
        {
            if (SimpleStorageService == null)
                SimpleStorageService = new SimpleStorageService(string.Empty, string.Empty, string.Empty);
            return SimpleStorageService;
        }


        public string GetOutputFileName() => string.Empty;

        public int GetMaxListSize() => MaxListSize;

        public bool HasMaxListSize() => GetMaxListSize() > 0;

        public static int MaxListSize { get; set; } = 5;

    }
}
