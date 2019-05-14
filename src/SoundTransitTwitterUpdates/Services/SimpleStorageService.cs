using System;
using System.IO;
using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using SoundTransitTwitterUpdates.Utilities;

namespace SoundTransitTwitterUpdates.Services
{
    public class SimpleStorageService : ISimpleStorageService
    {
        public IAmazonS3 Client { get; set; }

        private LambdaRestSerializer Serializer { get; set; }

        private string BucketName { get; set; }

        public SimpleStorageService(string keyId, string secretKey, string bucketName)
        {
            Client = new AmazonS3Client(keyId, secretKey, RegionEndpoint.USWest2);
            Serializer = new LambdaRestSerializer();

            BucketName = bucketName;
        }

        public T GetObject<T>(string fileName)
        {
            throw new NotImplementedException();
        }

        public bool PutObject<T>(T file, string fileName)
        {
            var stream = new MemoryStream();
            Serializer.Serialize<T>(file, stream);
            stream.Position = 0;

            var putRequest = new PutObjectRequest()
            {
                BucketName = this.BucketName,
                Key = fileName,
                ContentBody = new StreamReader(stream).ReadToEnd(),
                CannedACL = S3CannedACL.PublicRead
            };

            var response = Client.PutObjectAsync(putRequest).Result;

            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}
