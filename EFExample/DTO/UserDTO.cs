namespace EFExample.DTO
{
    public class UserDTO
    {
        public string? Username { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string AccountType { get; set; } = null!;

        public string? Password { get; set; }

    }
}
