using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Files;
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
    public class TaskFileService : ITaskFileService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBackblazeService _backblazeService;
        private readonly ILogger<TaskFileService> _logger;

        public TaskFileService(
            IRepositoryWrapper repositoryWrapper,
            IBackblazeService backblazeService,
            ILogger<TaskFileService> logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _backblazeService = backblazeService;
            _logger = logger;
        }

        public async Task<int> UploadFileAsync(int taskId, IFormFile file)
        {
            try
            {
                var uploadResult = await _backblazeService.UploadFileAsync(file, "taskFiles");

                var taskFile = new TaskFile
                {
                    TaskId = taskId,
                    FileName = file.FileName,
                    FilePath = uploadResult.FileKey
                };

                await _repositoryWrapper.TaskFile.Create(taskFile);
                await _repositoryWrapper.Save();

                return taskFile.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading task file for task {TaskId}", taskId);
                throw;
            }
        }

        public async Task<List<TaskFileInfo>> GetFilesByTaskIdAsync(int taskId)
        {
            var taskFiles = await _repositoryWrapper.TaskFile.GetByTaskIdAsync(taskId);

            var result = new List<TaskFileInfo>();
            foreach (var taskFile in taskFiles)
            {
                var fileUrl = await _backblazeService.GetFileUrlAsync(taskFile.FilePath);
                result.Add(new TaskFileInfo
                {
                    Id = taskFile.Id,
                    TaskId = taskFile.TaskId,
                    FileName = taskFile.FileName,
                    FileUrl = fileUrl
                });
            }

            return result;
        }

        public async Task<TaskFileInfo?> GetFileByIdAsync(int id)
        {
            var taskFile = await _repositoryWrapper.TaskFile.GetByIdAsync(id);
            if (taskFile == null) return null;

            var fileUrl = await _backblazeService.GetFileUrlAsync(taskFile.FilePath);

            return new TaskFileInfo
            {
                Id = taskFile.Id,
                TaskId = taskFile.TaskId,
                FileName = taskFile.FileName,
                FileUrl = fileUrl
            };
        }

        public async Task<bool> DeleteFileAsync(int id)
        {
            try
            {
                var taskFile = await _repositoryWrapper.TaskFile.GetByIdAsync(id);
                if (taskFile == null) return false;

                var deleteSuccess = await _backblazeService.DeleteFileAsync(taskFile.FilePath);
                if (!deleteSuccess)
                {
                    _logger.LogWarning("Failed to delete file from Backblaze B2: {FileKey}", taskFile.FilePath);
                }

                await _repositoryWrapper.TaskFile.Delete(taskFile);
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
            var taskFile = await _repositoryWrapper.TaskFile.GetByIdAsync(id);
            if (taskFile == null)
                throw new FileNotFoundException($"Task file with id {id} not found");

            var downloadResult = await _backblazeService.DownloadFileAsync(taskFile.FilePath);

            return new FileDownloadResult
            {
                Content = downloadResult.Content,
                ContentType = downloadResult.ContentType,
                FileName = taskFile.FileName
            };
        }
    }
}
