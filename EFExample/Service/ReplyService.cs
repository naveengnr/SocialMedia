using EFExample.Models;
using EFExample.DTO;
using EFExample.Interfaces;
using EFExample.Controllers;

namespace EFExample.Service
{
    public class ReplyService : Ireply
    {
        public readonly SocialMediaContext _context;
        public readonly ILogger<ReplyController> _logger;

        public ReplyService(SocialMediaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method using for the Update the Replies based on updateDTO parameter
        /// </summary>
        public async Task<String> UpdateReplies(ReplyUpdateDTO updateDTO)
        {
            try
            {
                var reply = _context.Replies.FirstOrDefault(e => e.ReplyId == updateDTO.ReplyId && e.IsDeleted == false);

                if (reply != null)
                {
                    reply.ReplyContent = updateDTO.Content;

                    await _context.SaveChangesAsync();

                    return "update successfull";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex.Message, ex);
                return ex.Message;
            }    

        }

        /// <summary>
        /// This method using for the Get the Replies based on ReplyId parameter
        /// </summary>

        public async Task<List<ReplyGetDTO>> GetReplies(int ReplyId)
        {
            try {
                IQueryable<Reply> reply = _context.Replies;

                if (ReplyId > 0)
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
                        Content = e.ReplyContent,
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
            catch (Exception ex) 
            {
                _logger?.LogError(ex.Message, ex);  

                return new List<ReplyGetDTO> { new ReplyGetDTO { ErrorMessage = ex.Message} };
            }

            }

        /// <summary>
        /// This method using for the Add the Reply based on reply parameter
        /// </summary>

        public async Task<String> AddReply(ReplyDTO reply)
        {
            try {
                bool replyExists = false;

                if (reply.ParentReplyId != null) {
                    replyExists = _context.Replies.Any(e => e.ReplyId == reply.ParentReplyId && e.IsDeleted == false);
                }
                else
                {
                    replyExists = true;
                }
                bool UserExists = _context.Users.Any(e => e.UserId == reply.UserId && e.IsDeleted == false);
                bool CommentExits = _context.Comments.Any(e => e.CommentId == reply.CommentId && e.IsDeleted == false);

                if (UserExists && CommentExits && replyExists)
                {
                    Reply replies = new Reply()
                    {
                        UserId = reply.UserId,
                        CommentId = reply.CommentId,
                        ReplyContent = reply.Content,
                        ParentReplyId = reply.ParentReplyId,
                        DateReplied = DateTime.Now
                    };

                    _context.Replies.Add(replies);
                    await _context.SaveChangesAsync();
                    return "Successfully Replied";
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
