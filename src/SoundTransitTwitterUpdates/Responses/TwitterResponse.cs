using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SoundTransitTwitterUpdates.Responses
{
    public class TwitterResponse : ITwitterResponse
    {
        [JsonProperty("id_str")]
        public string UID { get; set; }

        [JsonProperty("full_text")]
        public string FullText { get; set; }

        [JsonProperty("truncated")]
        public bool Truncated { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        [JsonConstructor]
        public TwitterResponse([JsonProperty("created_at")] string createdDate)
        {
            CreatedAt = DateTimeOffset.ParseExact(createdDate, "ddd MMM dd HH:mm:ss K yyyy", null);
        }

        public TwitterResponse()
        {
        }
    }
}