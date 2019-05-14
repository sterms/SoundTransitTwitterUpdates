using System;
using System.IO;
using SoundTransitTwitterUpdates.Configurations;
using SoundTransitTwitterUpdates.Responses;
using SoundTransitTwitterUpdates.Utilities;
using Amazon.Lambda.Core;
using RestSharp;

namespace SoundTransitTwitterUpdates.Services
{
    public class TokenService : ITokenService
    {
        private string tokenValue;

        public string Token => $"Bearer {TokenValue}";

        public bool IsTokenValid => !string.IsNullOrEmpty(tokenValue);

        public string TokenValue
        {
            get => IsTokenValid ? tokenValue : throw new NullReferenceException();
            set => tokenValue = value;
        }

        public TokenService(string tokenValue)
        {
            this.tokenValue = tokenValue;
        }
    }
}
