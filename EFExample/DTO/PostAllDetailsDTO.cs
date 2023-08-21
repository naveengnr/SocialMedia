namespace EFExample.DTO
{
    public class PostAllDetailsDTO
    {
        public string? PostContent { get; set; }
        public string? CommentContent { get; set; }
        public string? ReplytContent { get; set; }
        public int ShareCount { get; set; }
        public int PostLikesCount { get; set; }
        public int CommentLikesCount { get; set; }
        public int ReplyLikesCount { get; set; }

    }
}
