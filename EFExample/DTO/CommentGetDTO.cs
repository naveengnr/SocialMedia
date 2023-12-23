namespace EFExample.DTO
{
    public class CommentGetDTO
    {
        public int CommentId { get; set; }

        public int? CommentUserId { get; set; }

        public int? PostId { get; set; }

        public string? Content { get; set; }

        public DateTime? DateCommented { get; set; }

        public string EroorMessage { get; set; }    
    }
}
