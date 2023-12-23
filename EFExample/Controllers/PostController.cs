using EFExample.Service;
using Microsoft.AspNetCore.Mvc;
using EFExample.Models;
using EFExample.DTO;
using EFExample.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace EFExample.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class PostController : ControllerBase
    {
        public readonly Ipost _post;

        public PostController(Ipost post)
        {
            _post = post;
        }

        /// <summary>
        /// This API is For Getting All User Details By PostUserId or PostId
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///             Get
        ///             
        ///    {
        ///    
        ///       "PostUserId": 1000
        ///       
        ///    }
        ///    
        ///             or
        ///             
        ///    {
        ///        
        ///        "PostId": 100
        ///        
        ///    }
        ///    
        /// If we any of the value we get data related to it
        /// 
        /// </remarks>
        /// <param name="PostUserId"></param>
        /// <param name="PostId"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpGet("GetAllPosts")]
        public async Task<ActionResult> GetAllPosts(int PostId, int PostUserId)
        {
            var posts = await _post.GetPosts(PostId, PostUserId);

            if (posts != null)
            {
                return Ok(posts);
            }
            else
            {
                return NotFound("Posts Not Found");
            }
        }

        /// <summary>
        /// This API is For Updating Post Details
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///             Update
        ///             
        ///    {
        ///
        ///        "postId": 1001,
        ///        "content": "hi"
        ///       
        ///    }  
        ///   
        /// </remarks>
        /// <param name="updateDTO"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpPut("UpdatePost")]
        public async Task<ActionResult> UpdatePost(PostUpdateDTO updateDTO)
        {
            var UserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

            int Id = 0;

            if (UserId != null)
            {
                Id = Convert.ToInt32(UserId.Value);
            }


            var post = await _post.UpdatePost(updateDTO, Id);

            if (post != null)
            {
                return Ok(post);
            }
            else
            {
                return NotFound("post not found");
            }
        }

        /// <summary>
        /// This API is For Inserting NewPost Details
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///             Post
        ///    
        ///       {
        ///             "postUserId": 1001,
        ///             "content": "string"
        ///        }
        ///    
        /// </remarks>
        /// <param name="post"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpPost("newPost")]
        public async Task<IActionResult> AddPost(PostDTO post)
        {


            var UserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

            int Id = 0;

            if (UserId != null)
            {
                Id = Convert.ToInt32(UserId.Value);
            }

            var posts = await _post.AddPost(post, Id);

            return Ok(posts);
        }

        [HttpGet("GetAllPostsContent")]
        public ActionResult GetrAllContent()
        {
            var posts = _post.GetAllPostsContent();

            if (posts != null)
            {
                return Ok(posts);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// This Code is for GetAll Posts and Its Comments and Reply and ReplyReply 
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>
        /// 

        [HttpGet("GetPostsContent")]
        public async Task<ActionResult> GetPostContent()
        {
            var post = await _post.Posts();
            if (post != null)
            {
                return Ok(post);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
