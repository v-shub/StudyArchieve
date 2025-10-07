using Domain.Models;

namespace StudyArchieveApi.Contracts.Solution
{
    public class CreateSolutionRequest
    {
        public int TaskId { get; set; }

        public string SolutionText { get; set; } = null!;

        public int? UserAddedId { get; set; }
        public virtual ICollection<SolutionFile> SolutionFiles { get; set; } = new List<SolutionFile>();
    }
}
