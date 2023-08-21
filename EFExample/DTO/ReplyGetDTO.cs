namespace EFExample.DTO
{
    public class ReplyGetDTO
    {
        public int ReplyId { get; set; }

        public int? UserId { get; set; }

        public int? CommentId { get; set; }

        public string? Content { get; set; }

        public DateTime? DateReplied { get; set; }

        public int? ParentReplyId { get; set; }
    }
}
