using Domain.Models;
using StudyArchieveApi.Contracts.SolutionFile;
using StudyArchieveApi.Contracts.User;

namespace StudyArchieveApi.Contracts.Solution
{
    public class GetSolutionResponse
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public string SolutionText { get; set; } = string.Empty;

        public DateTime DateAdded { get; set; }

        public int? UserAddedId { get; set; }
        public virtual GetUserResponse? UserAdded { get; set; }
    }
}
