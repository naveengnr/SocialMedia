namespace EFExample.DTO
{
    public class PostContentDTO
    {
        public int PostId { get; set; }

        public int? PostUserId { get; set; }

        public string? PostContent { get; set; }

        public int PostLikes { get; set; }

        public List<CommentsDTO> comments { get; set; }

        public string ErrorMessage { get; set; }    
    }

    public class CommentsDTO
    {
        public int CommentId { get; set; }

        public int? CommentUserId { get; set; }

        public string? CommentContent { get; set; }

        public List<RepliesDTO> replies { get; set; }

    }

    public class RepliesDTO
    {
        public int ReplyId { get; set; }

        public int? ReplyUserId { get; set; }

        public string? ReplyContent { get; set; }

        public List<ReplyReplyDTO> replyreply { get; set; }
    }

    public class ReplyReplyDTO {
        public int? ParentReplyId { get; set; }

        public int? ReplyReplyUserId { get; set; }

        public string? ReplyReplyContent { get; set; }

    }

    public class LikeDTO
    {
        public int? LikeId { get; set; }

        public int? ReplyReplyUserId { get; set; }

        public string? ReplyReplyContent { get; set; }

    }
}


