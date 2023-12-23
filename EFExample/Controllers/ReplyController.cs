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
    [Authorize]
    public class ReplyController : ControllerBase
    {
        public readonly Ireply _reply;

        public ReplyController(Ireply reply , SocialMediaContext context)
        {
            _reply = reply;
        }

        /// <summary>
        /// This API is For Update Replies
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///           Update
        ///           
        ///           {
        ///           
        ///             "ReplyId" : 100,
        ///             "content":"hi"
        ///
        ///           }
        /// </remarks>
        /// <param name="updateDTO"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpPut("UpdateReplies")]
        public async Task<ActionResult> UpdateReplies(ReplyUpdateDTO updateDTO)
        {
            var reply = await _reply.UpdateReplies(updateDTO);

            if(reply != null)
            { 
                return Ok(reply);
            }
            else
            {
                return NotFound("Id not Found");
            }
        }

        /// <summary>
        /// This API is For Find Reply based RewplyId parameter 
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///           Get
        ///           
        ///           {
        ///           
        ///             "ReplyId" : 100
        ///             
        ///           }
        ///           
        /// If We Give ReplyId we get details of that id if we not given any id it gives all Replies
        /// </remarks>
        /// <param name="ReplyId"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>


        [HttpGet("GetReplies")]
        public async Task<ActionResult> GetReplies(int ReplyId)
        {
            var reply = await _reply.GetReplies(ReplyId);

            if(reply != null)
            {
                return Ok(reply);
            }
            else
            {
                return NotFound("id Not Found");
            }
        }

        /// <summary>
        /// This Code is for GetAllReplies
        /// </summary>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>
        /// 
        [HttpGet("GetALLReplies")]
        public async Task< ActionResult> GetRepliesAll()
        {
            int ReplyId = 0;
            var reply = await _reply.GetReplies(ReplyId);

            if (reply != null)
            {
                return Ok(reply);
            }
            else
            {
                return NotFound("id Not Found");
            }
        }

        /// <summary>
        /// This API is For Adding New Reply
        /// </summary>
        /// <remarks>
        /// Sample Request : 
        ///       Post
        /// 
        /// If you want to reply for comment
        /// 
        ///   {
        ///   
        ///      "userId": 1,
        ///      "commentId": 2,
        ///      "content": "hi"
        ///      
        ///   }
        ///   
        /// if you want to reply for Reply
        /// 
        ///   {
        ///   
        ///       "userId": 1,
        ///       "commentId":2,
        ///       "ParentReplyId":3,
        ///       "content":"hi"
        ///       
        ///   }
        ///    
        /// </remarks>
        /// <param name="reply"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpPost("addReply")]
        public async Task<ActionResult> AddReply(ReplyDTO reply)
        {
            var rep = await _reply.AddReply(reply);

            if(rep != null)
            {
               return Ok(rep);
            }
            else
            {
                return NotFound("user or Comment not exist");
            }
        }

        
    }
}
