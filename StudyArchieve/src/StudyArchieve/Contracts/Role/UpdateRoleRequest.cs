namespace StudyArchieveApi.Contracts.Role
{
    public class UpdateRoleRequest
    {
        public int Id { get; set; }

        public string RoleName { get; set; } = null!;
    }
}
