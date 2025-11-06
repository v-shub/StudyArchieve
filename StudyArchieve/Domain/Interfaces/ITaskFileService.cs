using Domain.Models.Files;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces
{
    public interface ITaskFileService
    {
        Task<int> UploadFileAsync(int taskId, IFormFile file);
        Task<List<TaskFileInfo>> GetFilesByTaskIdAsync(int taskId);
        Task<TaskFileInfo?> GetFileByIdAsync(int id);
        Task<bool> DeleteFileAsync(int id);
        Task<FileDownloadResult> DownloadFileAsync(int id);
    }
}
