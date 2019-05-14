using System;
using SoundTransitTwitterUpdates.Services;

namespace SoundTransitTwitterUpdates.Configurations
{
    public class ProductionConfiguration : IConfiguration
    {
        private TwitterService twitterService = null;
        public ITwitterService GetTwitterService()
        {
            if (twitterService == null)
                twitterService = new TwitterService(GetTokenService());
            return twitterService;
        }

        public ITokenService GetTokenService() => new TokenService(Environment.GetEnvironmentVariable(ConfigurationConstants.TokenKey));

        private SimpleStorageService simpleStorageService = null;
        public ISimpleStorageService GetSimpleStorageService()
        {
            if (simpleStorageService == null)
            {
                simpleStorageService = new SimpleStorageService(
                    Environment.GetEnvironmentVariable(ConfigurationConstants.AwsKeyId),
                    Environment.GetEnvironmentVariable(ConfigurationConstants.AwsSecretKey),
                    Environment.GetEnvironmentVariable(ConfigurationConstants.BucketKey));
            }
            return simpleStorageService;
        }

        public string GetOutputFileName() => Environment.GetEnvironmentVariable(ConfigurationConstants.FileNameKey);

        public int GetMaxListSize() => int.Parse(Environment.GetEnvironmentVariable(ConfigurationConstants.MaxListSizeKey));

        public bool HasMaxListSize() => GetMaxListSize() > 0;
    }
}
