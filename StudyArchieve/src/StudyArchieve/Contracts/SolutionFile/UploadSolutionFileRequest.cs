namespace StudyArchieveApi.Contracts.SolutionFile
{
    public class UploadSolutionFileRequest
    {
        public int SolutionId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
