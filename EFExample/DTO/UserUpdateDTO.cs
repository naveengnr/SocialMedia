namespace EFExample.DTO
{
    public class UserUpdateDTO
    {
        public int UserId { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Password { get; set; }
    }
}
