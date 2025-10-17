namespace StudyArchieveApi.Contracts.TaskFile
{
    public class UploadTaskFileResponse
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public long FileSize { get; set; }
    }
}
