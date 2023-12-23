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


        public FollowerController (Ifollower follower)
        {
            _follower = follower;
        }


        [HttpPost("addFollowing")]
        public ActionResult AddFollowing(FollowerDTO follower)
        {
            var followers = _follower.AddFollowing(follower);

            if(followers != null)
            {
                return Ok(_follower.AddFollowing(follower));
            }
            else
            {
                return NotFound("userid or followinguserid id not present");
            }
        }
    }
}
