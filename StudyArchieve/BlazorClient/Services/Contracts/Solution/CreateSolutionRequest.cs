namespace BlazorClient.Services.Contracts.Solution
{
    public class CreateSolutionRequest
    {
        public int TaskId { get; set; }

        public string SolutionText { get; set; } = null!;

        public int? UserAddedId { get; set; }
    }
}
