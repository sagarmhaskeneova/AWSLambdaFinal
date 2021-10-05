using Amazon;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWSLambdaFinal.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly ServerConfiguration _settings;
        private readonly IDynamoDBContext _dynamoDBContext;
        public FileRepository(IAmazonS3 amazons3, IOptions<ServerConfiguration> settings, IDynamoDBContext dynamoDBContext)
        {
            _settings = settings.Value;
            _amazonS3 = new AmazonS3Client(_settings.AWSS3.AccessKey,_settings.AWSS3.SecretKey, RegionEndpoint.USEast2);
            _dynamoDBContext = dynamoDBContext;
        }
        public async Task<ListObjectsV2Response> GetFileDataForDynamoDB()
        {
            ListObjectsV2Request request = new ListObjectsV2Request
            {
                BucketName = _settings.AWSS3.BucketName,
                MaxKeys = 30
            };
            ListObjectsV2Response response = await _amazonS3.ListObjectsV2Async(request);
            return response;
        }
        public async Task<GetObjectResponse> GetFile(String key)
        {

            var response = await _amazonS3.GetObjectAsync(this._settings.AWSS3.BucketName, key);
            return response;
        }

        public async Task Save(FileData fileData)
        {
            await _dynamoDBContext.SaveAsync(fileData);
        }
    }
}
