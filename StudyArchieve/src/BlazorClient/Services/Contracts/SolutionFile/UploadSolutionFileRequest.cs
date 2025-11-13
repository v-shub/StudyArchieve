using Microsoft.AspNetCore.Http;

namespace BlazorClient.Services.Contracts.SolutionFile
{
    public class UploadSolutionFileRequest
    {
        public int SolutionId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
