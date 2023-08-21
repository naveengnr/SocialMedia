using EFExample.Models;
using EFExample.DTO;
using EFExample.Interfaces;

namespace EFExample.Service
{
    public class ReplyService : Ireply
    {
        public readonly SocialMediaContext _context;

        public ReplyService(SocialMediaContext context)
        {
            _context = context;
        }

        public String UpdateReplies(ReplyUpdateDTO updateDTO)
        {
           
             var reply = _context.Replies.FirstOrDefault(e => e.ReplyId ==updateDTO.ReplyId && e.IsDeleted == false);

            if(reply != null)
            {
                reply.Content = updateDTO.Content;

                _context.SaveChanges();

                return "update successfull";
            }
            else
            {
                return null;
            }

        }


        public List<ReplyGetDTO> GetReplies(int ReplyId)
        {
            IQueryable<Reply> reply = _context.Replies;

            if(ReplyId < 0)
            {
                reply = reply.Where(e => e.ReplyId == ReplyId && e.IsDeleted == false);
            }

            var Replies = reply.ToList();

            if (Replies != null)
            {
                var r = Replies.Select(e => new ReplyGetDTO
                {
                    ReplyId = e.ReplyId,
                    CommentId = e.CommentId,
                     Content = e.Content,
                     UserId = e.UserId,
                     DateReplied = e.DateReplied,
                     ParentReplyId = e.ParentReplyId
                }).ToList();
                return r;
            }
            else
            {
                return null;
            }
        }

        public String AddReply(ReplyDTO reply)
        {
            bool replyExists = false;

            if (reply.ParentReplyId != null)              {
                replyExists = _context.Replies.Any(e => e.ReplyId == reply.ParentReplyId && e.IsDeleted == false);
            }
            else  
            {
                replyExists = true;  
            }
            bool UserExists = _context.Users.Any(e => e.UserId == reply.UserId && e.IsDeleted == false);
            bool CommentExits = _context.Comments.Any(e => e.CommentId == reply.CommentId && e.IsDeleted == false);

            if (UserExists && CommentExits && replyExists )
            {
                Reply replies = new Reply()
                {
                    UserId = reply.UserId,
                    CommentId = reply.CommentId,
                    Content = reply.Content,
                    ParentReplyId = reply.ParentReplyId,
                    DateReplied = DateTime.Now
                };

                _context.Replies.Add(replies);
                _context.SaveChanges();
                return "Successfully Replied";
            }
            else
            {
                return null;

            }
        }
    }
}
