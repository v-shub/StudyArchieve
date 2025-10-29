namespace BusinessLogic.Models.Files
{
    public class FileDownloadResult
    {
        public Stream Content { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public string FileName { get; set; } = null!;
    }
}
