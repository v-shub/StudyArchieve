using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces
{
    public interface ISolutionFileService
    {
        Task<int> UploadFileAsync(int taskId, IFormFile file);
        Task<List<SolutionFileInfo>> GetFilesBySolutionIdAsync(int solutionId);
        Task<SolutionFileInfo?> GetFileByIdAsync(int id);
        Task<bool> DeleteFileAsync(int id);
        Task<FileDownloadResult> DownloadFileAsync(int id);
    }
}
