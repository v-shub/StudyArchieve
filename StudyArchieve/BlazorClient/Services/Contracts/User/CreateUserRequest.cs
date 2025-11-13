namespace BlazorClient.Services.Contracts.User
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public int RoleId { get; set; }
    }
}
