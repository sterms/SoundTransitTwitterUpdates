using System;
using System.Linq;
using System.Text;
using SoundTransitTwitterUpdates.Responses;

namespace SoundTransitTwitterUpdates.Requests
{
    public static class AlexaRequestMapper
    {
        public static AlexaRequest Map(ITwitterResponse tweet)
        {
            int timeZone = -8;

            return new AlexaRequest()
            {
                UID = tweet.UID,
                UpdateDate = tweet.CreatedAt,
                TitleText = $"Sound Transit Tweet from {ApplyOffset(tweet.CreatedAt, timeZone).ToString("M/d/yy h:mm tt")}",
                MainText = $"{GenerateDayText(tweet.CreatedAt)}, {new AlexaRequestFormatter(tweet.FullText).Format()}",
                RedirectionUrl = $"http://www.twitter.com/SoundTransit/status/{tweet.UID}"
            };

            string GenerateDayText(DateTimeOffset createdAt)
            {
                var sB = new StringBuilder();

                if (ApplyOffset(createdAt, timeZone).Date == ApplyOffset(DateTimeOffset.Now, timeZone).Date)
                    sB.Append("Today");
                else if (ApplyOffset(createdAt, timeZone).Date == ApplyOffset(DateTimeOffset.Now, timeZone).AddDays(-1).Date)
                    sB.Append("Yesterday");
                else
                    sB.Append(createdAt.ToString("MMMM dd"));

                sB.Append(" at ");
                sB.Append(createdAt.DateTime.AddHours(-8).ToString("h:mm tt"));

                return sB.ToString();
            }

            DateTime ApplyOffset(DateTimeOffset originalDateTime, int offset)
            {
                if (originalDateTime.TimeOfDay.Hours < offset)
                    return originalDateTime.DateTime.AddDays(-1).AddHours(offset);
                else
                    return originalDateTime.DateTime.AddHours(offset);
            }
        }
    }
}
