namespace BlazorClient.Services.Contracts.TaskType
{
    public class UpdateTaskTypeRequest
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
