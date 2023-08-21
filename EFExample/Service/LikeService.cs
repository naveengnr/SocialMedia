using EFExample.DTO;
using EFExample.Interfaces;
using EFExample.Models;
using System.ComponentModel.Design.Serialization;

namespace EFExample.Service
{
    public class LikeService : Ilikes
    {
        public readonly SocialMediaContext _context;

        public LikeService(SocialMediaContext context)
        {
            _context = context;
        }

        public string AddLikes(LikesDTO likesDto)
        {
            var user = _context.Users.FirstOrDefault(e => e.UserId == likesDto.UserId);

            bool isPost = _context.Posts.Any(e => e.PostId == likesDto.LikeTypeId);
            bool isComment = _context.Comments.Any(e => e.CommentId == likesDto.LikeTypeId);
            bool isReply = _context.Replies.Any(e => e.ReplyId == likesDto.LikeTypeId);

            if (user != null && (isPost || isComment || isReply))
            {
                string likeType = isPost ? "Post" : (isComment ? "Comment" : "Reply");

                var like = new Like()
                {
                    UserId = likesDto.UserId,
                    LikeType = likeType,
                    LikeTypeId = likesDto.LikeTypeId,
                    DateLiked = DateTime.Now
                };
                _context.Likes.Add(like);
                _context.SaveChanges();
                return "Liked Successfully";
            }
            else
            {
                return null;
            }
        }

    }
}
