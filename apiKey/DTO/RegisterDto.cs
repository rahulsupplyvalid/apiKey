namespace masterapi.DTO
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }  // Store hashed passwords
        public string[] Roles { get; set; }  // Role of the user (Admin, User, etc.)
    }
}
