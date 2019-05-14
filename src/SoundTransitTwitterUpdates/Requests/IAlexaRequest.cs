using System;

namespace SoundTransitTwitterUpdates.Requests
{
    public interface IAlexaRequest
    {
        string UID { get; }

        DateTimeOffset UpdateDate { get; }

        string TitleText { get; }

        string MainText { get; }

        string RedirectionUrl { get; }
    }
}
