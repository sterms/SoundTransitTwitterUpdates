using System;
using System.Collections.Generic;

namespace SoundTransitTwitterUpdates.Responses
{
    public interface ITwitterResponse
    {
        DateTimeOffset CreatedAt { get; }
        string UID { get; }
        string FullText { get; }
        bool Truncated { get; }
    }
}
