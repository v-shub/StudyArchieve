using Domain.Models;
using StudyArchieveApi.Contracts.SolutionFile;

namespace StudyArchieveApi.Contracts.Solution
{
    public class GetSolutionResponse
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public string SolutionText { get; set; } = null!;

        public DateTime DateAdded { get; set; }

        public int? UserAddedId { get; set; }
    }
}
