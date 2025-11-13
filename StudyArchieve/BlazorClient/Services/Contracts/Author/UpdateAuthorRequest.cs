namespace BlazorClient.Services.Contracts.Author
{
    public class UpdateAuthorRequest
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
