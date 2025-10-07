using Domain.Models;

namespace StudyArchieveApi.Contracts.Task
{
    public class GetTaskResponse
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? ConditionText { get; set; }

        public int SubjectId { get; set; }

        public int? AcademicYearId { get; set; }

        public int TypeId { get; set; }

        public DateTime DateAdded { get; set; }

        public int? UserAddedId { get; set; }

        public virtual Domain.Models.AcademicYear? AcademicYear { get; set; }

        public virtual ICollection<Domain.Models.Solution> Solutions { get; set; } = new List<Domain.Models.Solution>();

        public virtual Domain.Models.Subject Subject { get; set; } = null!;

        public virtual ICollection<Domain.Models.TaskFile> TaskFiles { get; set; } = new List<Domain.Models.TaskFile>();

        public virtual Domain.Models.TaskType Type { get; set; } = null!;

        public virtual Domain.Models.User? UserAdded { get; set; }

        public virtual ICollection<Domain.Models.Author> Authors { get; set; } = new List<Domain.Models.Author>();

        public virtual ICollection<Domain.Models.Tag> Tags { get; set; } = new List<Domain.Models.Tag>();
    }
}
