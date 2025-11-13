namespace BlazorClient.Services.Contracts.Tag
{
    public class UpdateTagRequest
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
