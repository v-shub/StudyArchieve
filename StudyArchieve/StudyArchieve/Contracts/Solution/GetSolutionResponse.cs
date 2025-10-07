using Domain.Models;

namespace StudyArchieveApi.Contracts.Solution
{
    public class GetSolutionResponse
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public string SolutionText { get; set; } = null!;

        public DateTime DateAdded { get; set; }

        public int? UserAddedId { get; set; }
        public virtual ICollection<SolutionFile> SolutionFiles { get; set; } = new List<SolutionFile>();
    }
}
