using EFExample.DTO;
using EFExample.Interfaces;
using EFExample.Models;
using EFExample.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFExample.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
   // [Authorize]
    public class CommentController : ControllerBase
    {
        

        public readonly Icomments _comments;

        public CommentController ( Icomments comments)
        {
            
            _comments = comments;
        }
        /// <summary>
        /// This API is For Find CommentDetails 
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///           Get
        ///           
        ///           {
        ///             "CommentId" : 100
        ///           }
        ///           
        /// If We Give CommentsId we get details of that id if we not given any id it gives all comments
        /// </remarks>
        /// <param name="CommentId"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>


        [HttpGet("GetComments")]
        public async Task<ActionResult> GetComments(int CommentId)
        {
            
                var comments = await _comments.GetComments(CommentId);
                if (comments != null)
                {
                    return Ok(comments);
                }
                else
                {
                    return NotFound("CommentId NotFound");
                }
            
        }
        /// <summary>
        /// This API is For Update Comments
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///           Update
        ///           
        ///           {
        ///           
        ///             "CommentId" : 100,
        ///             "content":"hi"
        ///
        ///           }
        /// </remarks>
        /// <param name="updateDTO"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpPut("UpdateComments")]
        public async Task<ActionResult> UpdateComments(CommentUpdateDTO updateDTO)
        {
            var comments = await _comments.UpdateComment(updateDTO);
            if(comments != null)
            {
                return Ok(comments);
            }
            else
            {
                return NotFound("CommentId Not Found");
            }
        }

        /// <summary>
        /// This API is For Adding New Comment
        /// </summary>
        /// <remarks>
        /// Sample Request : 
        ///       Post
        ///       
        ///    {
        ///    
        ///        "commentUserId": 1000,
        ///        "postId": 101,
        ///        "content": "Hi All"
        ///        
        ///    }
        ///    
        /// </remarks>
        /// <param name="comment"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>


        [HttpPost("newComment")]
        [AllowAnonymous]
        public async Task<ActionResult> AddComment(CommentDTO comment)
        {
            var comments = await _comments.AddComment(comment);

            if(comments != null)
            {
                return Ok( comments);
            }
            else
            {
                return NotFound("PostID or UserID is Not Present");
            }
        }

    }
}
