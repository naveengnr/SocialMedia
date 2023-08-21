using EFExample.DTO;
using EFExample.Interfaces;
using EFExample.Models;
using EFExample.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShareController : ControllerBase
    {
        public readonly Ishare _share;

        public ShareController(Ishare share , SocialMediaContext context)
        {
            _share = share;
        }

        /// <summary>
        /// This API is For Find ShareDetails By UserId Or PostId
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///           Get
        ///           
        ///           {
        ///             "PostId" : 100
        ///           }
        ///           
        ///           Or
        ///           {
        ///               "UserId":1000
        ///           }
        /// 
        /// If We Give Both Only UserID  Will Work
        /// </remarks>
        /// <param name="UserId"></param>
        /// <param name="PostId"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpGet("GetShares")]
        public ActionResult GetShare(int UserId = 0 ,int PostId = 0)
        {

            if (UserId == 0 && PostId == 0)
            {
                return BadRequest("You must provide either userId or postId.");
            }

            var share = _share.GetShares( UserId, PostId);

            if (share == null)
            {
                return NotFound("user is not found");
            }
            else
            {
                return Ok(share);
            }
        }


        /// <summary>
        /// This API is For Delete Share Details By ShareId
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///           Delete
        ///           
        ///           {
        ///             "ShareId" : 100
        ///           }
        /// 
        /// </remarks>
        /// <param name="ShareId"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpDelete("DeleteShare")]
        public ActionResult DeleteShare(int ShareId)
        {
            var share = _share.DeleteShare(ShareId);
            if(share != null)
            {
                return Ok(share);
            }
            else
            {
                return NotFound("ShareId not Found");
            }
        }

        /// <summary>
        /// This API is For Adding Share Details
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///          Post
        ///          
        ///           {
        ///           
        ///            "UserId":1000,
        ///            "PostId" : 100
        ///            
        ///           }
        /// 
        /// 
        /// </remarks>
        /// <param name="share"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpPost("addShare")]
        public IActionResult Addshare(ShareDTO share)
        {
            var shares = _share.AddShare(share);

            if(shares != null)
            {
                return Ok(shares);

            }
            else
            {
                return BadRequest("Post Or User Not Found");
            }
        }
    }
}
