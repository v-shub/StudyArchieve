using StudyArchieveApi.Contracts.AcademicYear;
using StudyArchieveApi.Contracts.Solution;
using StudyArchieveApi.Contracts.Subject;
using StudyArchieveApi.Contracts.TaskType;
using StudyArchieveApi.Contracts.User;
using StudyArchieveApi.Contracts.Author;
using StudyArchieveApi.Contracts.Tag;
using StudyArchieveApi.Contracts.TaskFile;


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

        public virtual GetAcademicYearResponse? AcademicYear { get; set; }

        public virtual GetSubjectResponse Subject { get; set; } = null!;

        public virtual GetTaskTypeResponse Type { get; set; } = null!;

        public virtual GetUserResponse UserAdded { get; set; }

        public virtual ICollection<GetAuthorResponse> Authors { get; set; } = new List<GetAuthorResponse>();

        public virtual ICollection<GetTagResponse> Tags { get; set; } = new List<GetTagResponse>();
    }
}
