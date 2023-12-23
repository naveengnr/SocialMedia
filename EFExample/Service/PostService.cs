using EFExample.Models;
using Microsoft.AspNetCore.Mvc;
using EFExample.DTO;
using EFExample.Interfaces;
using Microsoft.EntityFrameworkCore;
using EFExample.Email;
using Serilog;
using EFExample.Controllers;

namespace EFExample.Service
{
    public class PostService : Ipost
    {
        public readonly SocialMediaContext _Context;
        public readonly ILogger<PostService> _logger;
        public readonly EmailService _emailService;
        public PostService(SocialMediaContext context, EmailService emailService, ILogger<PostService> logger)
        {
            _Context = context;
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// This method using for the Get the Post based on PostId Or PostUserId parameter
        /// </summary>
        public async Task<List<PostGetDTO>> GetPosts(int PostId = 0, int PostUserId = 0)
        {
            try
            {
                List<Post> post =  new List<Post>();
                post =await _Context.Posts.ToListAsync();

                if (PostId > 0)
                {
                    post =  post.Where(e => e.PostId == PostId && e.IsDeleted == false).ToList();
                }
                else if (PostUserId > 0)
                {
                    post = post.Where(e => e.PostUserId == PostUserId && e.IsDeleted == false).ToList();
                }


                var posts = post.ToList();

                if (posts != null)
                {

                    var p = posts.Select(e => new PostGetDTO
                    {
                        PostId = e.PostId,
                        PostUserId = e.PostUserId,
                        Content = e.PostContent,
                        DatePosted = e.DatePosted
                    }).ToList();

                    return p;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new List<PostGetDTO> { new PostGetDTO { ErrorMessage = ex.Message } };
            }
        }

        /// <summary>
        /// This method using for the Update the Post based on updateDTO parameter
        /// </summary>
        
        public async Task<string> UpdatePost(PostUpdateDTO updateDTO,int Id)
        {
            try
            {
                var post = _Context.Posts.FirstOrDefault(e => e.PostId == updateDTO.PostId && e.IsDeleted == false);

                if (post?.PostUserId == Id)
                {
                    if (post != null)
                    {

                        post.PostContent = updateDTO.Content;

                        await _Context.SaveChangesAsync();

                        return "Update Successfully";

                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return "UnAuthorised";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;
            }
        }



        /// <summary>
        /// This method using for the Add the Post based on post parameter
        /// </summary>
        public async Task<String> AddPost(PostDTO post , int Id)
        {
            try
            {
                if (post.PostUserId == Id)
                {
                    Post posts = new Post()
                    {
                        PostUserId = post.PostUserId,
                        PostContent = post.Content,
                        DatePosted = DateTime.Now
                    };
                    _Context.Posts.Add(posts);
                    var friends =
                                 (from f in _Context.Followers
                                  join u in _Context.Users on f.UserId equals u.UserId
                                  where (posts.PostUserId == f.UserId)
                                  select new
                                  {
                                      postuser = u.Username,
                                      followersid = f.FollowerUserId
                                  }
                                 into k
                                  join fo in _Context.Users on k.followersid equals fo.UserId
                                  select new
                                  {
                                      puser = k.postuser,
                                      username = fo.Username,
                                      email = fo.Email

                                  });


                    string? SenderName = null;
                    string? Receiveremail = null;
                    string? Receivername = null;
                    foreach (var item in friends)
                    {
                        SenderName = item.puser;
                        Receiveremail = item.email;
                        Receivername = item.username;
                    }

                    _emailService.SendEmail (Receiveremail ,"newPost", Receivername, SenderName);

                    await _Context.SaveChangesAsync();
                }
                else
                {
                    return "UnAuthorised";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;
            }
            return ("Post created successfully");
        }


        public List<PostAllDetailsDTO> GetAllPostsContent()
        {

            var allDetailsList = _Context.Posts.Select(post => new PostAllDetailsDTO
            {
                PostContent = post.PostContent,
                PostLikesCount = _Context.Likes.Count(like => like.LikeType == "post" && like.LikeTypeId == post.PostId),
                ShareCount = post.Shares.Count(),
                CommentLikesCount = _Context.Likes.Count(like => like.LikeType == "comment" && like.LikeTypeId == post.PostId),
                CommentContent = string.Join(" /n ", post.Comments.Select(comment => comment.CommentContent)),
                ReplytContent = string.Join(" /n ", post.Comments.SelectMany(comment => comment.Replies).Select(reply => reply.ReplyContent)),
                ReplyLikesCount = _Context.Likes.Count(like => like.LikeType == "reply" && like.LikeTypeId == post.PostId),

            }).ToList();

            return allDetailsList;
        }

        /// <summary>
        /// This method using for the GetAll the Posts and its comments and reply and replyreply 
        /// </summary>

        public async Task<IQueryable<PostContentDTO>> Posts()
        {
            try
            {
                var posts = from p in _Context.Posts
                            orderby p.DatePosted
                            select new PostContentDTO
                            {
                                PostId = p.PostId,
                                PostUserId = p.PostUserId,
                                PostContent = p.PostContent,
                                comments = (from c in _Context.Comments
                                            where c.PostId == p.PostId
                                            orderby c.DateCommented
                                            select new CommentsDTO
                                            {
                                                CommentId = c.CommentId,
                                                CommentUserId = c.CommentUserId,
                                                CommentContent = c.CommentContent,
                                                replies = (from r in _Context.Replies
                                                           where r.CommentId == c.CommentId
                                                           orderby r.DateReplied
                                                           select new RepliesDTO
                                                           {
                                                               ReplyId = r.ReplyId,
                                                               ReplyUserId = r.UserId,
                                                               ReplyContent = r.ReplyContent,
                                                               replyreply = (from rr in _Context.Replies
                                                                             where rr.ParentReplyId == r.ReplyId
                                                                             orderby rr.DateReplied
                                                                             select new ReplyReplyDTO
                                                                             {
                                                                                 ParentReplyId = rr.ParentReplyId,
                                                                                 ReplyReplyUserId = rr.UserId,
                                                                                 ReplyReplyContent = rr.ReplyContent
                                                                             }).ToList()
                                                           }).ToList()
                                            }).ToList()
                            };

                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return new List<PostContentDTO> { new PostContentDTO { ErrorMessage = ex.Message } }.AsQueryable();
            }

        }

    }
}
