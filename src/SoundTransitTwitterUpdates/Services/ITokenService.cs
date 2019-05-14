using System;
namespace SoundTransitTwitterUpdates.Services
{
    public interface ITokenService
    {
        bool IsTokenValid { get; }

        string TokenValue { get; set; }

        string Token { get; }
    }
}
