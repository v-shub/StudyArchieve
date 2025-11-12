using StudyArchieveApi.Contracts.Author;
using StudyArchieveApi.Contracts.Tag;

namespace StudyArchieveApi.Contracts.Task
{
    public class CreateTaskRequest
    {
        public string Title { get; set; } = null!;
        public string? ConditionText { get; set; }
        public int SubjectId { get; set; }
        public int? AcademicYearId { get; set; }
        public int TypeId { get; set; }
        public int? UserAddedId { get; set; }

        // Только ID для связей
        public List<int> AuthorIds { get; set; } = new List<int>();
        public List<int> TagIds { get; set; } = new List<int>();
    }
}
