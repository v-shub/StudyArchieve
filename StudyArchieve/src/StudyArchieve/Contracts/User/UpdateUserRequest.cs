using Domain.Models;

namespace StudyArchieveApi.Contracts.User
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public int RoleId { get; set; }
    }
}
