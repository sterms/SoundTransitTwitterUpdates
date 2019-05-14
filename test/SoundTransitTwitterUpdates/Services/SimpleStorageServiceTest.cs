using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using SoundTransitTwitterUpdates.Requests;
using SoundTransitTwitterUpdates.Services;
using SoundTransitTwitterUpdates.Utilities;
using Xunit;

namespace SoundTransitTwitterUpdates.Tests.Services
{
    public class SimpleStorageServiceTest
    {
        /// <summary>
        /// Tests that if PutObject gets a bad request status back, it returns false.
        /// </summary>
        [Fact]
        public void PutObject_WithBadRequest_False()
        {
            // Arrange
            var s3Mock = new Mock<IAmazonS3>();
            var service = new SimpleStorageService(string.Empty, string.Empty, string.Empty);

            // Act
            s3Mock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new PutObjectResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest
            }));
                  
            service.Client = s3Mock.Object;

            // Assert
            Assert.False(service.PutObject<string>(It.IsAny<string>(), It.IsAny<string>()));
        }

        /// <summary>
        /// Tests that an error PutObject throws up will propagate up.
        /// </summary>
        [Fact]
        public void PutObject_WithError_ThrowsError()
        {
            // Arrange
            var s3Mock = new Mock<IAmazonS3>();
            var service = new SimpleStorageService(string.Empty, string.Empty, string.Empty);

            // Act
            s3Mock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            service.Client = s3Mock.Object;

            // Assert
            Assert.Throws<Exception>(() => service.PutObject<string>(It.IsAny<string>(), It.IsAny<string>()));
        }

        /// <summary>
        /// Tests that if everything ok, PutObject returns a true;
        /// </summary>
        [Fact]
        public void PutObject_WithOk_True()
        {
            // Arrange
            var s3Mock = new Mock<IAmazonS3>();
            var service = new SimpleStorageService(string.Empty, string.Empty, string.Empty);

            // Act
            s3Mock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new PutObjectResponse()
            {
                HttpStatusCode = HttpStatusCode.OK
            }));

            service.Client = s3Mock.Object;

            // Assert
            Assert.True(service.PutObject<string>(It.IsAny<string>(), It.IsAny<string>())); 
        }

        /// <summary>
        /// Tests that the PutObject function uses a serializer, and properly.
        /// </summary>
        [Fact]
        public void PutObject_SerializesData_True()
        {
            // Arrange
            var s3Mock = new Mock<IAmazonS3>();
            var service = new SimpleStorageService(string.Empty, string.Empty, string.Empty);

            var stream = new MemoryStream();
            var serializer = new LambdaRestSerializer();

            string serializedContentFromService = null;
            string serializedContentFromTest = null;
            var mockRequests = new List<AlexaRequest>()
            {
                new AlexaRequest()
                {
                    UID = "1",
                    MainText = "Hello there",
                    TitleText = "Test 1",
                    UpdateDate = DateTime.Now,
                    RedirectionUrl = "http://www.google.com"
                },
                new AlexaRequest()
                {
                    UID = "2",
                    MainText = "Bye",
                    TitleText = "Test 2",
                    UpdateDate = DateTime.Now.AddHours(-1),
                    RedirectionUrl = "http://www.amazon.com"
                }
            };

            serializer.Serialize<List<AlexaRequest>>(mockRequests, stream);
            stream.Position = 0;
            serializedContentFromTest = new StreamReader(stream).ReadToEnd();

            // Act
            s3Mock
                .Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
                .Callback((PutObjectRequest request, CancellationToken token) => 
                {
                    serializedContentFromService = request.ContentBody;
                })
                .Returns(Task.FromResult(new PutObjectResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK
                }));

            service.Client = s3Mock.Object;

            service.PutObject<List<AlexaRequest>>(mockRequests, It.IsAny<string>());

            // Assert
            Assert.Equal(serializedContentFromTest, serializedContentFromService); 
        }
    }
}
