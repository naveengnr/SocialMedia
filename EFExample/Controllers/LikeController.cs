using EFExample.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EFExample.DTO;
using Microsoft.AspNetCore.Authorization;

namespace EFExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LikeController : ControllerBase
    {
        public readonly Ilikes _like;

        public LikeController(Ilikes like)
        {
            _like = like;
        }
        /// <summary>
        /// This Code is for AddLikes Based on likesDto Parameter
        /// </summary>
        /// <remarks>
        ///        {
        ///        
        ///           "userId" : 1019,
        ///           "LikeTypeId" : 1000
        ///           
        ///        }
        /// 
        /// </remarks>
        /// <param name="likesDto"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>
        /// 
        [HttpPost("addLikes")]
        [AllowAnonymous]
        public async Task< ActionResult> AddLike(LikesDTO likesDto)
        {
            var like = await _like.AddLikes(likesDto);
            if(like != null)
            {
                return Ok(like);
            }
            else
            {
                return NotFound("id not found");
            }
        }
    }
}
