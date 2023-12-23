namespace EFExample.DTO
{
    public class UserDeleteDTO
    {
        public bool UserIsDeleted { get; set; }
        public bool PostIsDeleted { get; set; }
        public bool CommentIsDeleted { get; set; }
        public bool ReplyIsDeleted { get; set; }
        public bool LikeIsDeleted { get; set; }
        public bool ShareIsDeleted { get; set; }
    }
}
