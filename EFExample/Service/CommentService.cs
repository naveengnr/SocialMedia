using EFExample.DTO;
using EFExample.Models;
using EFExample.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EFExample.Controllers;
using EFExample.Email;

namespace EFExample.Service
{
    public class CommentService : Icomments
    {
        public readonly SocialMediaContext _context;
        public readonly ILogger<CommentService> _logger;
        public readonly EmailService _emailService;

        public CommentService(SocialMediaContext context, EmailService emailService , ILogger<CommentService> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }
        /// <summary>
        /// This method using for the GetComments based on CommentId parameter
        /// </summary>
        public async Task<List<CommentGetDTO>> GetComments(int CommentId)
        {
            try {
                IQueryable<Comment> comment = _context.Comments;
                if (CommentId > 0)
                {
                    comment = comment.Where(e => e.CommentId == CommentId && e.IsDeleted == false);
                }

                var Comments = comment.ToList();

                if (Comments != null)
                {

                    var c = Comments.Select(e => new CommentGetDTO
                    {
                        CommentUserId = e.CommentUserId,
                        CommentId = e.CommentId,
                        PostId = e.PostId,
                        Content = e.CommentContent,
                        DateCommented = e.DateCommented
                    }).ToList();
                    return c;
                }
                else
                {
                    return null;
                }

            }catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                return new List<CommentGetDTO> { new CommentGetDTO { EroorMessage = ex.Message } };
             }
            }

        /// <summary>
        /// This method using for the Update the Comment based on UpdateDto parameter
        /// </summary>
        public async Task<string> UpdateComment(CommentUpdateDTO updateDTO)
        {
            try
            {
                var comment = _context.Comments.FirstOrDefault(e => e.CommentId == updateDTO.CommentId && e.IsDeleted == false);

                if (comment != null)
                {
                    comment.CommentContent = updateDTO.Content;

                    await _context.SaveChangesAsync();

                    return "Update Successfull";
                }
                else
                {
                    return null;
                }
            }catch(Exception ex)
            {
                _logger?.LogError(ex.Message, ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// This method using for the Add Comment based on Comment parameter
        /// </summary>
        public async Task<String> AddComment(CommentDTO comment)
        {
            try {
                bool UserExists = _context.Users.Any(e => e.UserId.Equals(comment.CommentUserId));
                bool PostExits = _context.Posts.Any(e => e.PostId.Equals(comment.PostId));

                if (UserExists && PostExits)
                {
                    var comments = new Comment()
                    {
                        CommentUserId = comment.CommentUserId,
                        CommentContent = comment.Content,
                        PostId = comment.PostId,
                        DateCommented = DateTime.Now

                    };

                    _context.Comments.Add(comments);

                    var Email = (from p in _context.Posts
                                where (p.PostId == comment.PostId )
                                select new
                                {
                                    user = p.PostUserId
                                } 
                                into k
                                join u in _context.Users on k.user equals u.UserId
                                select new
                                {
                                    userid = u.UserId,
                                    username = u.Username,
                                    useremail = u.Email
                                }
                                into j
                                 join u in _context.Users on comments.CommentUserId equals u.UserId
                                 select new
                                 {
                                     commentusername = u.Username,
                                     commentuseremail = u.Email,
                                     postusername = j.username,
                                     postuseremail = j.useremail

                                 });
                    string SenderName = null;
                    string Receiveremail = null;
                    string Receivername = null;

                    foreach(var i in Email)
                    {
                        SenderName = i.commentusername;
                        Receiveremail = i.postuseremail;
                        Receivername = i.postusername;        
                    }
                    _emailService.SendEmail(Receiveremail, "newComment", Receivername, SenderName);

                    await _context.SaveChangesAsync();
                    return " commented";
                }
                else
                {
                    return null;
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return ex.Message;
            }

            }

    }
}
