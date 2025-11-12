namespace StudyArchieveApi.Contracts.SolutionFile
{
    public class SolutionFileResponse
    {
        public int Id { get; set; }
        public int SolutionId { get; set; }
        public string FileName { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public long FileSize { get; set; }
    }
}