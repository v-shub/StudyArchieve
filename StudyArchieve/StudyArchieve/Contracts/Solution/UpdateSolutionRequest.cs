using Domain.Models;

namespace StudyArchieveApi.Contracts.Solution
{
    public class UpdateSolutionRequest
    {
        public int Id { get; set; }

        public string SolutionText { get; set; } = null!;
        public virtual ICollection<Domain.Models.SolutionFile> SolutionFiles { get; set; } = new List<Domain.Models.SolutionFile>();
    }
}
