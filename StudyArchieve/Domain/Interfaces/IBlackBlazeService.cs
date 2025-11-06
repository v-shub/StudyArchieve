using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces
{
    public interface IBackblazeService
    {
        Task<FileUploadResult> UploadFileAsync(IFormFile file, string folder);
        Task<string> GetFileUrlAsync(string fileKey);
        Task<bool> DeleteFileAsync(string fileKey);
        Task<FileDownloadResult> DownloadFileAsync(string fileKey);
    }
}
