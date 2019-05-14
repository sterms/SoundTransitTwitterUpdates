using System;
using System.IO;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Deserializers;

namespace SoundTransitTwitterUpdates.Utilities
{
    public class LambdaRestSerializer : ILambdaSerializer, IDeserializer
    {
        protected JsonSerializer Serializer { get; }

        public string RootElement { get; set; }

        public string Namespace { get; set; }

        public string DateFormat { get; set; }

        public LambdaRestSerializer()
        {
            Serializer = new JsonSerializer();
            Serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public T Deserialize<T>(Stream requestStream)
        {
            return Serializer.Deserialize<T>(new JsonTextReader(new StreamReader(requestStream)));
        }

        public void Serialize<T>(T response, Stream responseStream)
        {
            var sw = new StreamWriter(responseStream);
            Serializer.Serialize(sw, response, typeof(T));
            sw.Flush();
        }

        public T Deserialize<T>(IRestResponse response)
        {
            return Serializer.Deserialize<T>(new JsonTextReader(new StringReader(response.Content)));
        }
    }
}
