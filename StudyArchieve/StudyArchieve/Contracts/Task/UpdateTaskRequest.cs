using Domain.Models;

namespace StudyArchieveApi.Contracts.Task
{
    public class UpdateTaskRequest
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? ConditionText { get; set; }

        public int SubjectId { get; set; }

        public int? AcademicYearId { get; set; }

        public int TypeId { get; set; }
        public virtual ICollection<Domain.Models.Author> Authors { get; set; } = new List<Domain.Models.Author>();

        public virtual ICollection<Domain.Models.Tag> Tags { get; set; } = new List<Domain.Models.Tag>();
    }
}
