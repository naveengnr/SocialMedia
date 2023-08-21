using EFExample.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EFExample.DTO;

namespace EFExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikeController : ControllerBase
    {
        public readonly Ilikes _like;

        public LikeController(Ilikes like)
        {
            _like = like;
        }
        [HttpPost("addLikes")]
        public ActionResult AddLike(LikesDTO likesDto)
        {
            var like = _like.AddLikes(likesDto);
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
