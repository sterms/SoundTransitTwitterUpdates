using System;
using Newtonsoft.Json;

namespace SoundTransitTwitterUpdates.Requests
{
    public class AlexaRequest : IAlexaRequest
    {
        public string UID { get; set; }

        [JsonConverter(typeof(AlexaDateTimeJsonConverter))]
        public DateTimeOffset UpdateDate { get; set; }

        public string TitleText { get; set; }

        public string MainText { get; set; }

        public string RedirectionUrl { get; set; }

        public static AlexaRequest Empty
        {
            get
            {
                return new AlexaRequest()
                {
                    UID = Guid.Empty.ToString(),
                    UpdateDate = DateTimeOffset.Now,
                    TitleText = "No new updates",
                    MainText = "No new updates",
                    RedirectionUrl = "http://www.twitter.com/SoundTransit"
                };   
            }
        }
    }
}
