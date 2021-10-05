using Amazon.DynamoDBv2.DataModel;
using AWSLambdaFinal.Repository;
using AWSLambdaFinal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace AWSLambdaFinal.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileService _fileServices;
        private readonly ILogger<FileController> _logger;
        public FileController(IFileRepository fileRepo, FileService fileServices, ILogger<FileController> logger)
        {
            _fileServices = fileServices;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<FileData> fileDataList = await _fileServices.GetFiles_Service();
                if (fileDataList == null)
                    throw new NullReferenceException("FileData is null");
                foreach (var fileData in fileDataList)
                {
                    await this._fileServices.SaveFilesInfo_Service(fileData);
                    _logger.LogInformation("Data Save Successfully in DynamoDB");
                }
                _logger.LogInformation("Get method executed Successfully");
                return Ok(fileDataList);
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "FileDataList getting null in get method");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Some unknown error has occurred.");
                return BadRequest();
            }
        }
    }
}
