using System;
using Amazon.S3;

namespace SoundTransitTwitterUpdates.Services
{
    public interface ISimpleStorageService
    {
        IAmazonS3 Client { get; set; }

        bool PutObject<T>(T file, string fileName);

        T GetObject<T>(string fileName);
    }
}
