using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoundTransitTwitterUpdates.Configurations;
using SoundTransitTwitterUpdates.Requests;
using SoundTransitTwitterUpdates.Responses;
using SoundTransitTwitterUpdates.Utilities;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(LambdaRestSerializer))]

namespace SoundTransitTwitterUpdates
{
    public class Function
    {
        public IConfiguration Configuation { get; set; }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void FunctionHandler(JObject input, ILambdaContext context)
        {
            if (Configuation == null)
            {
                var config = Environment.GetEnvironmentVariable(ConfigurationConstants.ConfigurationKey);
                Configuation = GetConfiguration(config);
            }

            IEnumerable<IAlexaRequest> requests = ConvertToAlexaRequest(Configuation.GetTwitterService().GetRecentTweets(12)).OrderBy(x => x.UpdateDate);

            // Truncate lists longer than N size.
            if (Configuation.HasMaxListSize() && requests.Count() > Configuation.GetMaxListSize())
                requests = requests.Skip(requests.Count() - Configuation.GetMaxListSize());

            if (!Configuation.GetSimpleStorageService().PutObject<IEnumerable<IAlexaRequest>>(
               requests.Any() ? requests : new List<IAlexaRequest>() { AlexaRequest.Empty }, 
                Configuation.GetOutputFileName()))
            {
                throw new Exception("File output failed with Unknown error.");
            }
        }

        private IConfiguration GetConfiguration(string configurationName)
        {
            switch (configurationName)
            {
                case ConfigurationConstants.ConfigurationProductionValue:
                    return new ProductionConfiguration();
                case ConfigurationConstants.ConfigurationQAValue:
                    return new QAConfiguration();
                case ConfigurationConstants.ConfigurationDevelopmentValue:
                default:
                    return new DevelopmentConfiguration();
            }
        }

        private IEnumerable<IAlexaRequest> ConvertToAlexaRequest(IEnumerable<ITwitterResponse> tweets) => tweets.Select(x => AlexaRequestMapper.Map(x));
    }
}
