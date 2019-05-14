using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Amazon.S3;
using RestSharp;
using RestSharp.Serializers;
using SoundTransitTwitterUpdates.Utilities;

namespace SoundTransitTwitterUpdates.Services
{
    public class SimpleStorageServiceStub : ISimpleStorageService
    {
        private Dictionary<string, string> Storage { get; set; } = new Dictionary<string, string>();

        private LambdaRestSerializer Serializer { get; set; } = new LambdaRestSerializer();

        public IAmazonS3 Client { get; set; }

        public T GetObject<T>(string fileName)
        {
            if (!Storage.ContainsKey(fileName))
                return default(T);

            var stream = new MemoryStream();
            Serializer.Serialize<string>(Storage[fileName], stream);

            return Serializer.Deserialize<T>(stream);                
        }

        public bool PutObject<T>(T file, string fileName)
        {
            var stream = new MemoryStream();
            Serializer.Serialize<T>(file, stream);
            stream.Position = 0;

            var stringContent = new StreamReader(stream).ReadToEnd();

            if (Storage.ContainsKey(fileName))
                Storage[fileName] = stringContent;
            else
                Storage.Add(fileName, stringContent);

            return true;
        }
    }
}
