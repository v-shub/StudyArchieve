namespace BusinessLogic.Models.Files
{
    public class FileUploadResult
    {
        public string FileKey { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public long FileSize { get; set; }
    }
}
