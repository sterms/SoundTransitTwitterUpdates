using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using SoundTransitTwitterUpdates;
using SoundTransitTwitterUpdates.Requests;
using RestSharp;
using Amazon.S3;
using Moq;
using SoundTransitTwitterUpdates.Configurations;
using SoundTransitTwitterUpdates.Responses;
using Amazon.S3.Model;
using System.Threading;
using SoundTransitTwitterUpdates.Services;

namespace SoundTransitTwitterUpdates.Tests
{
    public class FunctionTest
    {
        /// <summary>
        /// Tests that a basic run of the FunctionHandler returns true.
        /// </summary>
        [Fact]
        public void FunctionHandler_ReturnsResponses_True()
        {
            // Arrange
            var function = new Function();
            var context = new TestLambdaContext();
            bool result = true;

            // Act
            try
            {
                function.FunctionHandler(null, context);
            }
            catch (Exception)
            {
                result = false;
            }

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests that a list larger than 5, will eventually be truncated by this function after it's 
        /// twitter service calls, but before it's s3 service calls.
        /// </summary>
        [Fact]
        public void FunctionHandler_TruncatesListGreaterThanMaxSize_True()
        {
            // Arrange
            var restMock = new Mock<IRestClient>();
            var simpleServiceMock = new Mock<ISimpleStorageService>();
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

            IEnumerable<IAlexaRequest> formattedList = null;

            var function = new Function();
            var context = new TestLambdaContext();
            var config = new QAConfiguration();

            // Act
            restMock
                .Setup(x => x.Execute<List<TwitterResponse>>(It.IsAny<RestRequest>()))
                .Returns(new RestResponse<List<TwitterResponse>>
                {
                    Data = twitterResponseMock,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    ResponseStatus = ResponseStatus.Completed
                });

            simpleServiceMock
                .Setup(x => x.PutObject<IEnumerable<IAlexaRequest>>(It.IsAny<IEnumerable<IAlexaRequest>>(), It.IsAny<string>()))
                .Callback((IEnumerable<IAlexaRequest> file, string fileName) =>
                {
                    formattedList = file;
                })
                .Returns(true);

            config.TokenService = new TokenService("Test");
            config.TwitterService = new TwitterService(config.TokenService);
            config.TwitterService.Client = restMock.Object;
            config.SimpleStorageService = simpleServiceMock.Object;

            function.Configuation = config;
            function.FunctionHandler(null, context);

            // Assert
            Assert.Equal(5, formattedList.Count());
            Assert.Equal(6, twitterResponseMock.Count());
        }

        /// <summary>
        /// Tests that an error in S3 propagates.
        /// </summary>
        [Fact]
        public void FunctionHandler_WhenS3HasError_ThrowsError()
        {
            // Arrange
            var simpleServiceMock = new Mock<ISimpleStorageService>();

            var function = new Function();
            var context = new TestLambdaContext();
            var config = new QAConfiguration();

            // Act
            simpleServiceMock
                .Setup(x => x.PutObject<IEnumerable<IAlexaRequest>>(It.IsAny<IEnumerable<IAlexaRequest>>(), It.IsAny<string>()))
                .Throws(new Exception());

            config.TwitterService = new TwitterServiceStub();
            config.SimpleStorageService = simpleServiceMock.Object;

            function.Configuation = config;

            // Assert
            Assert.Throws<Exception>(() => function.FunctionHandler(null, context));
        }

        /// <summary>
        /// Tests that bad status from the S3 service results in an error.
        /// </summary>
        [Fact]
        public void FunctionHandler_WhenS3HasBadStatus_ThrowsError()
        {
            // Arrange
            var simpleServiceMock = new Mock<ISimpleStorageService>();

            var function = new Function();
            var context = new TestLambdaContext();
            var config = new QAConfiguration();

            // Act
            simpleServiceMock
                .Setup(x => x.PutObject<IEnumerable<IAlexaRequest>>(It.IsAny<IEnumerable<IAlexaRequest>>(), It.IsAny<string>()))
                .Returns(false);

            config.TwitterService = new TwitterServiceStub();
            config.SimpleStorageService = simpleServiceMock.Object;

            function.Configuation = config;

            // Assert
            Assert.Throws<Exception>(() => function.FunctionHandler(null, context));
        }

        /// <summary>
        /// Tests if twitter throws an error, it propagates.
        /// </summary>
        [Fact]
        public void FunctionHandler_WhenTwitterHasError_ThrowsError()
        {
            // Arrange
            var twitterMock = new Mock<ITwitterService>();

            var function = new Function();
            var context = new TestLambdaContext();
            var config = new QAConfiguration();

            // Act
            twitterMock
                .Setup(x => x.GetRecentTweets(It.IsAny<int>()))
                .Throws(new Exception());

            config.TwitterService = new TwitterServiceStub();
            config.TwitterService = twitterMock.Object;

            function.Configuation = config;

            // Assert
            Assert.Throws<Exception>(() => function.FunctionHandler(null, context));
        }
    }
}
