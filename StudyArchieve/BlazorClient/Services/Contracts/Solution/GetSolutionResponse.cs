using BlazorClient.Services.Contracts.User;

namespace BlazorClient.Services.Contracts.Solution
{
    public class GetSolutionResponse
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public string SolutionText { get; set; } = null!;

        public DateTime DateAdded { get; set; }

        public int? UserAddedId { get; set; }
        public virtual GetUserResponse? UserAdded { get; set; }
    }
}
