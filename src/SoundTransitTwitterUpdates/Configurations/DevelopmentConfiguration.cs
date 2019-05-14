using System;
using SoundTransitTwitterUpdates.Services;

namespace SoundTransitTwitterUpdates.Configurations
{
    public class DevelopmentConfiguration : IConfiguration
    {
        public ITwitterService GetTwitterService() => new TwitterServiceStub();

        public ITokenService GetTokenService() => throw new NotImplementedException();

        public ISimpleStorageService GetSimpleStorageService() => new SimpleStorageServiceStub();

        public string GetOutputFileName() => "text.txt";

        public int GetMaxListSize() => 5;

        public bool HasMaxListSize() => true;
    }
}
