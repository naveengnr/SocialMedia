namespace EFExample.DTO
{
    public class PostGetDTO
    {
        public int PostId { get; set; }

        public int? PostUserId { get; set; }

        public string? Content { get; set; }

        public DateTime? DatePosted { get; set; }
    }
}
