namespace EFExample.DTO
{
    public class ReplyDTO
    {
        public int? UserId { get; set; }

        public int? CommentId { get; set; }
        public int? ParentReplyId { get; set; }
        public string? Content { get; set; }
    }
}
