using Microsoft.AspNetCore.Http;

namespace BlazorClient.Services.Contracts.TaskFile
{
    public class UploadTaskFileRequest
    {
        public int TaskId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
