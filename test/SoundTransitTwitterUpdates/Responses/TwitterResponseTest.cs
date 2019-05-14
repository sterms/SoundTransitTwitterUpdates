using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SoundTransitTwitterUpdates.Responses;
using SoundTransitTwitterUpdates.Utilities;
using Xunit;

namespace SoundTransitTwitterUpdates.Tests
{
    public class TwitterResponseTest
    {
        /// <summary>
        /// Tests that the Twitter Response correctly deserializes using a json constructor.
        /// </summary>
        [Fact]
        public void Deserialize_UsesJsonProperties_True()
        {
            // Arrange
            var testResponse = new TestResponse()
            {
                id_str = "123",
                created_at = "Sun Dec 16 20:56:33 +0000 2018",
                full_text = "Wello Horld! http://www.Steve.com",
                truncated = false,
            };
            var serializer = new LambdaRestSerializer();
            var stream = new MemoryStream();
            TwitterResponse result = null;

            // Act
            serializer.Serialize<TestResponse>(testResponse, stream);
            stream.Position = 0;
            result = serializer.Deserialize<TwitterResponse>(stream);

            // Assert
            Assert.Equal(result.UID, testResponse.id_str);
            Assert.Equal(result.CreatedAt, new DateTimeOffset(2018, 12, 16, 20, 56, 33, new TimeSpan(0)));
            Assert.Equal(result.FullText, testResponse.full_text);
            Assert.Equal(result.Truncated, testResponse.truncated);
        }

        private class TestResponse
        {
            public string id_str { get; set; }
            public string created_at { get; set; }
            public string full_text { get; set; }
            public bool truncated { get; set; }
        }
    }
}
