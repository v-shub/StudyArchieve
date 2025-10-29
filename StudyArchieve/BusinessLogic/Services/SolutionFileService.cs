using BusinessLogic.Models.Files;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Services
{
    // BusinessLogic/Services/TaskFileService.cs
    public class SolutionFileService : ISolutionFileService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBackblazeService _backblazeService;
        private readonly ILogger<SolutionFileService> _logger;

        public SolutionFileService(
            IRepositoryWrapper repositoryWrapper,
            IBackblazeService backblazeService,
            ILogger<SolutionFileService> logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _backblazeService = backblazeService;
            _logger = logger;
        }

        public async Task<int> UploadFileAsync(int solutionId, IFormFile file)
        {
            try
            {
                var uploadResult = await _backblazeService.UploadFileAsync(file, "solutionFiles");

                var solutionFile = new SolutionFile
                {
                    SolutionId = solutionId,
                    FileName = file.FileName,
                    FilePath = uploadResult.FileKey
                };

                await _repositoryWrapper.SolutionFile.Create(solutionFile);
                await _repositoryWrapper.Save();

                return solutionFile.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading task file for task {TaskId}", solutionId);
                throw;
            }
        }

        public async Task<List<SolutionFileInfo>> GetFilesBySolutionIdAsync(int solutionId)
        {
            var solutionFiles = await _repositoryWrapper.SolutionFile.GetBySolutionIdAsync(solutionId);

            var result = new List<SolutionFileInfo>();
            foreach (var solutionFile in solutionFiles)
            {
                var fileUrl = await _backblazeService.GetFileUrlAsync(solutionFile.FilePath);
                result.Add(new SolutionFileInfo
                {
                    Id = solutionFile.Id,
                    SolutionId = solutionFile.SolutionId,
                    FileName = solutionFile.FileName,
                    FileUrl = fileUrl
                });
            }

            return result;
        }

        public async Task<SolutionFileInfo?> GetFileByIdAsync(int id)
        {
            var solutionFile = await _repositoryWrapper.SolutionFile.GetByIdAsync(id);
            if (solutionFile == null) return null;

            var fileUrl = await _backblazeService.GetFileUrlAsync(solutionFile.FilePath);

            return new SolutionFileInfo
            {
                Id = solutionFile.Id,
                SolutionId = solutionFile.SolutionId,
                FileName = solutionFile.FileName,
                FileUrl = fileUrl
            };
        }

        public async Task<bool> DeleteFileAsync(int id)
        {
            try
            {
                var solutionFile = await _repositoryWrapper.SolutionFile.GetByIdAsync(id);
                if (solutionFile == null) return false;

                var deleteSuccess = await _backblazeService.DeleteFileAsync(solutionFile.FilePath);
                if (!deleteSuccess)
                {
                    _logger.LogWarning("Failed to delete file from Backblaze B2: {FileKey}", solutionFile.FilePath);
                }

                await _repositoryWrapper.SolutionFile.Delete(solutionFile);
                await _repositoryWrapper.Save();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task file with id {FileId}", id);
                return false;
            }
        }

        public async Task<FileDownloadResult> DownloadFileAsync(int id)
        {
            var solutionFile = await _repositoryWrapper.SolutionFile.GetByIdAsync(id);
            if (solutionFile == null)
                throw new FileNotFoundException($"Task file with id {id} not found");

            var downloadResult = await _backblazeService.DownloadFileAsync(solutionFile.FilePath);

            return new FileDownloadResult
            {
                Content = downloadResult.Content,
                ContentType = downloadResult.ContentType,
                FileName = solutionFile.FileName
            };
        }
    }
}
