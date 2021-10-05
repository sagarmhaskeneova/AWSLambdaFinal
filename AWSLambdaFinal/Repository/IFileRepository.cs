using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWSLambdaFinal.Repository
{
    public interface IFileRepository
    {
        Task<ListObjectsV2Response> GetFileDataForDynamoDB();

        Task<GetObjectResponse> GetFile(string filename);
        Task Save(FileData fileData);
    }
}
