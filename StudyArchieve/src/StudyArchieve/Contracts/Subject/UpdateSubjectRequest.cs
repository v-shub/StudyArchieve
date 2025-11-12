namespace StudyArchieveApi.Contracts.Subject
{
    public class UpdateSubjectRequest
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
