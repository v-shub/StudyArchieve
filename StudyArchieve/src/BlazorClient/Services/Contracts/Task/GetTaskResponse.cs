using BlazorClient.Services.Contracts.Subject;
using BlazorClient.Services.Contracts.AcademicYear;
using BlazorClient.Services.Contracts.TaskType;
using BlazorClient.Services.Contracts.Author;
using BlazorClient.Services.Contracts.User;
using BlazorClient.Services.Contracts.Tag;


namespace BlazorClient.Services.Contracts.Task
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
