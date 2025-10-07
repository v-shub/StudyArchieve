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

        public virtual ICollection<Domain.Models.TaskFile> TaskFiles { get; set; } = new List<Domain.Models.TaskFile>();
        public virtual ICollection<Domain.Models.Author> Authors { get; set; } = new List<Domain.Models.Author>();

        public virtual ICollection<Domain.Models.Tag> Tags { get; set; } = new List<Domain.Models.Tag>();
    }
}
