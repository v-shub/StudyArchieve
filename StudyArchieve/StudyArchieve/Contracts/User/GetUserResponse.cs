using Domain.Models;

namespace StudyArchieveApi.Contracts.User
{
    public class GetUserResponse
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public int RoleId { get; set; }

        public virtual Domain.Models.Role Role { get; set; } = null!;
    }
}
