namespace BlazorClient.Services.Contracts.Role
{
    public class UpdateRoleRequest
    {
        public int Id { get; set; }

        public string RoleName { get; set; } = null!;
    }
}
