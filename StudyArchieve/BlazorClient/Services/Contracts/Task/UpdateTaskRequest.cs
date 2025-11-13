namespace BlazorClient.Services.Contracts.Task
{
    public class UpdateTaskRequest
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? ConditionText { get; set; }

        public int SubjectId { get; set; }

        public int? AcademicYearId { get; set; }

        public int TypeId { get; set; }
        public List<int> AuthorIds { get; set; } = new List<int>();
        public List<int> TagIds { get; set; } = new List<int>();
    }
}
