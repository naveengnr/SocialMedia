namespace EFExample.DTO
{
    public class GetAllDTO
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateUserJoined { get; set; }

    }
}
