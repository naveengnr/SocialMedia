using EFExample.Service;
using Microsoft.AspNetCore.Mvc;
using EFExample.Models;
using EFExample.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using EFExample.Interfaces;
using EFExample.Secutiy;
using EFExample.Email;
using Serilog;
namespace EFExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        public readonly Iuser _iuser;
        public UserController(Iuser iuser )
        {
            _iuser = iuser;

        }

        /// <summary>
        /// This API is For Getting All User Details
        /// </summary>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpGet("GetAll")]
        
        public async Task<IActionResult> GetAll()
        {
           
            var user = await _iuser.GetAll();

            if (user == null)
            {
                return NotFound("no data found");
            }
            else
            {
                return Ok(user);
            }
        }

        /// <summary>
        /// This API is For Getting All User Details By UserId
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///             Get
        ///             
        ///    {
        ///    
        ///       "userId": 1000,
        ///       
        ///    }
        ///    
        /// </remarks>
        /// <param name="UserId"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpGet("GetById")]
        public async Task<ActionResult> GetById(int UserId)
        {

        
            var users = await _iuser.GetById(UserId);

            if(users != null)
            {
                return Ok(users);
            }
            else
            {
                return NotFound("userid not present");
            }
        }

        /// <summary>
        /// This API is For Delete user By UserId
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///             Delete
        ///             
        ///    {
        ///    
        ///       "userId": 1000,
        ///       
        ///    }
        ///    
        /// </remarks>
        /// <param name="UserId"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>
        
        [HttpDelete]
       
        public async Task<ActionResult> DeleteUser(int UserId)
        {
            var UsersId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

            int Id = 0;

            if (UserId != null)
            {
                Id = Convert.ToInt32(UsersId.Value);
            }
            var users = await _iuser.DeleteUser(UserId , Id);

            if(users != null)
            {
                return Ok(users);
            }
            else
            {
                return NotFound("Userid not found");
            }
        }

        /// <summary>
        /// This API is For Getting All User Details By Username
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///             Get
        ///             
        ///    {
        ///    
        ///       "username": "Mike",
        ///       
        ///    }
        ///    
        /// </remarks>
        /// <param name="Username"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>
        
        [HttpGet("GetByName")]
        public async Task<ActionResult> GetByName(string Username)
        {
            var users = await _iuser.GetByName(Username);

            if (users != null)
            {
                return Ok(users);
            }
            else
            {
                return NotFound("username not present");
            }
        }

        /// <summary>
        /// This API is For Updating User Details
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///             Update
        ///             
        ///    {
        ///    
        ///       "userId": 1111,
        ///       "username": "mike",
        ///       "email": "mike@gmail.com",
        ///       "dateOfBirth": "2023-08-18",
        ///       "password":"mike1234"
        ///       
        ///    }
        ///    
        /// We can also change single change Example :
        /// 
        ///     {
        ///    
        ///       "userId": 1111,
        ///       "username": "string",
        ///       "email": "mike@gmail.com",
        ///       "dateOfBirth": "2023-08-18",
        ///       "password":"string"
        ///       
        ///    }
        /// 
        ///    
        /// </remarks>
        /// <param name="updateDTO"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpPut("updateById")]
       
        public async Task<ActionResult> UpdateById(UserUpdateDTO updateDTO)
        {
            var UserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

            int Id = 0;

            if (UserId != null)
            {
                Id = Convert.ToInt32(UserId.Value);
            }

            var result = await _iuser.UpdateById(updateDTO , Id);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("User not found or update failed");
            }
        }



        /// <summary>
        /// This API is For Inserting NewUser Details
        /// </summary>
        /// <remarks>
        /// Sample Request :
        ///             Post
        ///             
        ///    {
        ///    
        ///       "userId": 1111,
        ///       "username": "mike",
        ///       "email": "mike@gmail.com",
        ///       "dateOfBirth": "2023-08-18",
        ///       "password":"mike1234"
        ///       
        ///    } 
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpPost("newUser")]
        [AllowAnonymous]
        public async Task<ActionResult> newUser(UserDTO user)
        {
            return Ok(await _iuser.newUser(user));
        }

        [HttpPost("ProfilePicture")]
        public ActionResult AddProfilePicture(int UserId, IFormFile profilePicture)
        {
            if (profilePicture == null || profilePicture.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var profile = _iuser.AddProfilePicture(UserId, profilePicture);

            if (profile == null)
            {
                return NotFound("User NotFound");
            }
            else
            {
                return Ok(profile);
            }
        }
        [HttpGet("ProfilePic")]
        [Produces("application/octet-stream")]
        //[Produces("image/png")]
        [AllowAnonymous]
        public ActionResult GetProfilePic(int UserId)
        {
            var profile = _iuser.GetProfilePictureByUserId(UserId);
            if(profile == null)
            {
                return NotFound();
            }
            else
            {
                return File(profile, "application/octet-stream");
            }
        }
    }
}

