using Amazon.S3.Model;
using AWSLambdaFinal.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AWSLambdaFinal.Services
{
    public class FileService
    {
        IFileRepository _fileRepository;
        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public async Task<FileData> GetFile_service(string key)
        {
            try
            {
                int wordCnt = 0;
                var response = await _fileRepository.GetFile(key);
                FileData fileData = new FileData();
                Stream responseStream = response.ResponseStream;
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string responseBody = reader.ReadToEnd();
                    if (responseBody == string.Empty)
                        wordCnt = 0;
                    else
                    {
                        string[] arr = responseBody.Split(new char[] { ' ', ',' });
                        foreach (string Spltword in arr)
                        {
                            wordCnt++;
                        }
                    }
                    fileData.FileInputCount = wordCnt;
                    fileData.FileName = key;

                }
                return fileData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveFilesInfo_Service(FileData fileData)
        {
            try
            {
                await _fileRepository.Save(fileData);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<FileData>> GetFiles_Service()
        {
            try
            {
                var response = await _fileRepository.GetFileDataForDynamoDB();
                List<FileData> lst = new List<FileData>();
                foreach (S3Object entry in response.S3Objects)
                {
                    if (entry.Key.Contains(".txt"))
                    {
                        string key = entry.Key;
                        FileData response_Catalog = await GetFile_service(key);
                        lst.Add(response_Catalog);
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}
