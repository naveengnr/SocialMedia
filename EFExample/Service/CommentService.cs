using EFExample.DTO;
using EFExample.Models;
using EFExample.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EFExample.Service
{
    public class CommentService : Icomments
    {
        public readonly SocialMediaContext _context;

        public CommentService(SocialMediaContext context)
        {
            _context = context;
        }

        public List<CommentGetDTO> GetComments(int CommentId)
        {
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
                    Content = e.Content,
                    DateCommented = e.DateCommented
                }).ToList();
                return c;
            }
            else
            {
                return null;
            }

            }

        public string UpdateComment(CommentUpdateDTO updateDTO)
        {
            var comment = _context.Comments.FirstOrDefault(e => e.CommentId == updateDTO.CommentId && e.IsDeleted == false);

            if (comment != null)
            {
                comment.Content = updateDTO.Content;

                _context.SaveChanges();

                return "Update Successfull";
            }
            else
            {
                return null;
            }
        }


        public String AddComment(CommentDTO comment)
        {
            bool UserExists = _context.Users.Any(e => e.UserId.Equals(comment.CommentUserId));
            bool PostExits = _context.Posts.Any(e => e.PostId.Equals(comment.PostId));

            if (UserExists && PostExits)
            {
                var comments = new Comment()
                {
                    CommentUserId = comment.CommentUserId,
                    Content = comment.Content,
                    PostId = comment.PostId,
                    DateCommented = DateTime.Now

                };

                _context.Comments.Add(comments);
                _context.SaveChanges();

                return "successfully commented";
            }
            else
            {
                return null;
            }
        }
    }
}
