using Amazon.S3;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Amazon.S3.Model;

namespace BusinessLogic.Services
{
    public class BackblazeService : IBackblazeService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly ILogger<BackblazeService> _logger;

        public BackblazeService(IAmazonS3 s3Client, IConfiguration config, ILogger<BackblazeService> logger)
        {
            _s3Client = s3Client;
            _bucketName = config["Backblaze:BucketName"]!;
            _logger = logger;
        }

        public async Task<FileUploadResult> UploadFileAsync(IFormFile file, string folder)
        {
            var fileKey = $"{folder}/{Guid.NewGuid()}_{SanitizeFileName(file.FileName)}";

            using (var stream = file.OpenReadStream())
            {
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileKey,
                    InputStream = stream,
                    ContentType = file.ContentType,
                    AutoCloseStream = false
                };

                var response = await _s3Client.PutObjectAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var fileUrl = await GetFileUrlAsync(fileKey);

                    return new FileUploadResult
                    {
                        FileKey = fileKey,
                        FileUrl = fileUrl,
                        FileSize = file.Length
                    };
                }
                else
                {
                    throw new Exception("Failed to upload file to Backblaze B2");
                }
            }
        }

        public async Task<string> GetFileUrlAsync(string fileKey)
        {
            return $"https://{_bucketName}.s3.us-west-002.backblazeb2.com/{fileKey}";
        }

        public async Task<bool> DeleteFileAsync(string fileKey)
        {
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileKey
                };

                var response = await _s3Client.DeleteObjectAsync(request);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file from Backblaze B2: {FileKey}", fileKey);
                return false;
            }
        }

        public async Task<FileDownloadResult> DownloadFileAsync(string fileKey)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey
            };

            var response = await _s3Client.GetObjectAsync(request);

            return new FileDownloadResult
            {
                Content = response.ResponseStream,
                ContentType = response.Headers.ContentType,
                FileName = Path.GetFileName(fileKey)
            };
        }

        private string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
