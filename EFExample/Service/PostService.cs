using EFExample.Models;
using Microsoft.AspNetCore.Mvc;
using EFExample.DTO;
using EFExample.Interfaces;

namespace EFExample.Service
{
    public class PostService : Ipost
    {
        public readonly SocialMediaContext _Context;

        public PostService(SocialMediaContext context)
        {
            _Context = context;
        }

        public List<PostGetDTO> GetPosts(int PostId = 0, int PostUserId = 0)
        {
            IQueryable<Post> post = _Context.Posts;

            if (PostId > 0)
            {
                post = post.Where(e => e.PostId == PostId && e.IsDeleted == false);
            }
            else if (PostUserId > 0)
            {
                post = post.Where(e => e.PostUserId == PostUserId && e.IsDeleted == false);
            }


            var posts = post.ToList();

            if (posts != null)
            {

                var p = posts.Select(e => new PostGetDTO
                {
                    PostId = e.PostId,
                    PostUserId = e.PostUserId,
                    Content = e.Content,
                    DatePosted = e.DatePosted
                }).ToList();

                return p;
            }
            else
            {
                return null;
            }
        }

        public string UpdatePost(PostUpdateDTO updateDTO)
        {
            var post = _Context.Posts.FirstOrDefault(e => e.PostId == updateDTO.PostId && e.IsDeleted == false);

            if (post != null)
            {

                post.Content = updateDTO.Content;

                _Context.SaveChanges();

                return "Update Successfull";

            }
            else
            {
                return null;
            }
        }

        public String AddPost(PostDTO post)
        {
            Post posts = new Post()
            {
                PostUserId = post.PostUserId,
                Content = post.Content,
                DatePosted = DateTime.Now
            };

            _Context.Posts.Add(posts);
            _Context.SaveChanges();

            return "Post created succesfully";
        }


        public List<PostAllDetailsDTO> GetAllPostsContent()
        {
            using (var context = new SocialMediaContext())
            {
                var allDetailsList = context.Posts.Select(post => new PostAllDetailsDTO
                {
                    PostContent = post.Content,
                    PostLikesCount = context.Likes.Count(like => like.LikeType == "post" && like.LikeTypeId == post.PostId),
                    ShareCount = post.Shares.Count(),
                    CommentLikesCount = context.Likes.Count(like => like.LikeType == "comment" && like.LikeTypeId == post.PostId),
                    CommentContent = string.Join(" /n ", post.Comments.Select(comment => comment.Content) ),
                    ReplytContent = string.Join(" /n ", post.Comments.SelectMany(comment => comment.Replies).Select(reply => reply.Content)),
                    ReplyLikesCount = context.Likes.Count(like => like.LikeType == "reply" && like.LikeTypeId == post.PostId),
                }).ToList();

                return allDetailsList;
            }
        }

    }
}
