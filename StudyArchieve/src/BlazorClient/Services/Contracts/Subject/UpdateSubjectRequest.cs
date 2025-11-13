namespace BlazorClient.Services.Contracts.Subject
{
    public class UpdateSubjectRequest
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
