using BlazorClient.Services.Contracts.Role;

namespace BlazorClient.Services.Contracts.User
{
    public class GetUserResponse
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public int RoleId { get; set; }

        public virtual GetRoleResponse Role { get; set; } = null!;
    }
}
