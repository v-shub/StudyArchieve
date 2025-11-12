using Domain.Models;
using StudyArchieveApi.Contracts.User;

namespace StudyArchieveApi.Contracts.Solution
{
    public class CreateSolutionRequest
    {
        public int TaskId { get; set; }

        public string SolutionText { get; set; } = null!;

        public int? UserAddedId { get; set; }
    }
}
