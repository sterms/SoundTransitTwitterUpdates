using System;
using SoundTransitTwitterUpdates.Services;

namespace SoundTransitTwitterUpdates.Configurations
{
    public interface IConfiguration
    {
        ITwitterService GetTwitterService();

        ITokenService GetTokenService();

        ISimpleStorageService GetSimpleStorageService();

        string GetOutputFileName();

        int GetMaxListSize();

        bool HasMaxListSize();
    }
}
