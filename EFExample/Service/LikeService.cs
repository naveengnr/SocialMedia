using EFExample.DTO;
using EFExample.Email;
using EFExample.Interfaces;
using EFExample.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design.Serialization;

namespace EFExample.Service
{
    public class LikeService : Ilikes
    {
        public readonly SocialMediaContext _context;
        public readonly ILogger<LikeService> _logger;
        public readonly EmailService _emailservice;
        public LikeService(SocialMediaContext context , ILogger<LikeService> logger, EmailService emailservice)
        {
            _context = context;
            _logger = logger;
            _emailservice = emailservice;
        }

        public async Task<string> AddLikes(LikesDTO likesDto)
        {
            try {
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
                    await _context.SaveChangesAsync();
                    return "Liked Successfully";
                }
                else
                {
                    return null;
                }
            }catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                
                return ex.Message;
            }
            }
    }
}
