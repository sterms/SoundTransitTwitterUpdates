using System;
using System.Collections.Generic;
using System.Net;
using Moq;
using RestSharp;
using SoundTransitTwitterUpdates.Responses;
using SoundTransitTwitterUpdates.Services;
using Xunit;

namespace SoundTransitTwitterUpdates.Tests.Services
{
    public class TwitterServiceTest
    {
        /// <summary>
        /// Tests that a bad request will throw an error.
        /// </summary>
        [Fact]
        public void GetRecentTweets_WithBadRequest_ThrowsError()
        {
            // Arrange
            var restMock = new Mock<IRestClient>();
            var service = new TwitterService(new TokenService("Test"));

            // Act
            restMock
                .Setup(x => x.Execute<List<TwitterResponse>>(It.IsAny<RestRequest>()))
                .Returns(new RestResponse<List<TwitterResponse>>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ResponseStatus = ResponseStatus.Error
                });

            service.Client = restMock.Object;

            // Assert
            Assert.Throws<ArgumentException>(() => service.GetRecentTweets(It.IsAny<int>()));
        }

        /// <summary>
        /// Tests that an Unauthorized response will chuck an error.
        /// </summary>
        [Fact]
        public void GetRecentTweets_WithUnauthorized_ThrowsError()
        {
            // Arrange
            var restMock = new Mock<IRestClient>();
            var service = new TwitterService(new TokenService("Test"));

            // Act
            restMock
                .Setup(x => x.Execute<List<TwitterResponse>>(It.IsAny<RestRequest>()))
                .Returns(new RestResponse<List<TwitterResponse>>()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ResponseStatus = ResponseStatus.Error
                });

            service.Client = restMock.Object;

            // Assert
            Assert.Throws<ArgumentException>(() => service.GetRecentTweets(It.IsAny<int>()));
        }

        /// <summary>
        /// Tests that a OK response will return a list of Tweets.
        /// </summary>
        [Fact]
        public void GetRecentTweets_WithOK_True()
        {
            // Arrange
            var restMock = new Mock<IRestClient>();
            var service = new TwitterService(new TokenService("Test"));
            var twitterResponseMock = new List<TwitterResponse>()
            {
                new TwitterResponse()
                {
                    UID = "1",
                    CreatedAt = DateTime.Now.AddHours(-1),
                    FullText = "Test Data",
                    Truncated = false
                },
                new TwitterResponse()
                {
                    UID = "2",
                    CreatedAt = DateTime.Now.AddHours(-1),
                    FullText = "Test Data",
                    Truncated = false
                },
                new TwitterResponse()
                {
                    UID = "3",
                    CreatedAt = DateTime.Now.AddHours(-1),
                    FullText = "Test Data",
                    Truncated = false
                },
                new TwitterResponse()
                {
                    UID = "4",
                    CreatedAt = DateTime.Now.AddHours(-1),
                    FullText = "Test Data",
                    Truncated = false
                },
                new TwitterResponse()
                {
                    UID = "5",
                    CreatedAt = DateTime.Now.AddHours(-1),
                    FullText = "Test Data",
                    Truncated = false
                },
                new TwitterResponse()
                {
                    UID = "6",
                    CreatedAt = DateTime.Now.AddHours(-1),
                    FullText = "Test Data",
                    Truncated = false
                }
            };

            // Act
            restMock
                .Setup(x => x.Execute<List<TwitterResponse>>(It.IsAny<RestRequest>()))
                .Returns(new RestResponse<List<TwitterResponse>>()
                {
                    StatusCode = HttpStatusCode.OK,
                    ResponseStatus = ResponseStatus.Completed,
                    Data = twitterResponseMock
                });

            service.Client = restMock.Object;

            // Assert
            Assert.Equal(twitterResponseMock, service.GetRecentTweets(12));
        }
    }
}
