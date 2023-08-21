using EFExample.DTO;
using EFExample.Interfaces;
using EFExample.Models;
using EFExample.Service;
using Microsoft.AspNetCore.Mvc;

namespace EFExample.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class FollowerController : ControllerBase
    {
        public readonly Ifollower _follower;
        public readonly SocialMediaContext _context;

        public FollowerController (FollowerService service)
        {
            _follower = service;
        }
        [HttpPost("addFollowing")]
        public ActionResult AddFollowing(FollowerDTO follower)
        {
            bool UserExists = _context.Users.Any(e => e.UserId.Equals(follower.UserId));
            bool FollowerExists = _context.Users.Any(e => e.UserId.Equals(follower.FollowingUserId));

            if(UserExists && FollowerExists)
            {
                return Ok(_follower.AddFollowing(follower));
            }
            else
            {
                return BadRequest("userid or followinguserid id not present");
            }
        }
    }
}
