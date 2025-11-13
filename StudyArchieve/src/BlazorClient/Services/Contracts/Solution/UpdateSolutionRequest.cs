namespace BlazorClient.Services.Contracts.Solution
{
    public class UpdateSolutionRequest
    {
        public int Id { get; set; }

        public string SolutionText { get; set; } = null!;
    }
}
