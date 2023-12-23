using EFExample.DTO;
using EFExample.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FriendController : ControllerBase
    {
        public readonly Ifriend _friend;

        public FriendController(Ifriend ifriend)
        {
            _friend = ifriend;
        }

        /// <summary>
        /// This Code is for Send Friend Request 
        /// </summary>
        /// <remarks>
        ///        {
        ///        
        ///           "userId" : 1019,
        ///           "FollowerId" : 1000
        ///           
        ///        }
        /// 
        /// </remarks>
        /// <param name="friendDTO"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>
        
        [HttpPost("addfriend")]
        public async Task<ActionResult> AddFriend(FriendDTO friendDTO)
        {
            var friend = await _friend.AddFriend(friendDTO);
            if(friend == null)
            {
                return NotFound("not found");
            }
            else
            {
                return Ok(friend);
            }
        }

        /// <summary>
        /// This Code is for Accept  Friend Request 
        /// </summary>
        /// <remarks>
        ///        {
        ///        
        ///           "userId" : 1019,
        ///           "FollowerId" : 1000
        ///           
        ///        }
        /// 
        /// </remarks>
        /// <param name="friendDTO"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>
        /// 
        [HttpPut("accept Request")]
        public async Task<ActionResult> AcceptRequest(FriendDTO friendDTO)
        {
            var friend = await _friend.AcceptRequest(friendDTO);

            if(friend == null)
            {
                return NotFound("not found");
            }
            else
            {
                return Ok(friend);
            }
        }
    }
}
