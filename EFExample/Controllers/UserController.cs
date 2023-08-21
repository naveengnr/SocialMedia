using EFExample.Service;
using Microsoft.AspNetCore.Mvc;
using EFExample.Models;
using EFExample.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using EFExample.Interfaces;

namespace EFExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public readonly Iuser _iuser;

        //public readonly SocialMediaContext _context;
        public UserController(Iuser iuser)
        {
            _iuser = iuser;
            //_context = context;
        }


        //[HttpGet("GetAllUsers")]
        //[AllowAnonymous]
        //public IActionResult GetAllUsers()
        //{
        //    var u = _iuser.GetAllUsers();

        //    if (u == null)
        //    {
        //        return NotFound("No data found");
        //    }
        //    else
        //    {
        //        return Ok(u);
        //    }

        //}

        /// <summary>
        /// This API is For Getting All User Details
        /// </summary>
        /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
        /// <response code="200">Returns when the update operation is successful.</response>
        /// <response code="400">Returns when the input data is invalid or the request is malformed.</response>
        /// <response code="404">Returns when the user with the specified ID is not found.</response>

        [HttpGet("GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            var user = _iuser.GetAll();

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
        public ActionResult GetById(int UserId)
        {
            var users = _iuser.GetById(UserId);

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
        public ActionResult DeleteUser(int UserId)
        {
            var users = _iuser.DeleteUser(UserId);

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
        public ActionResult GetByName(string Username)
        {
            var users = _iuser.GetByName(Username);

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
        public ActionResult UpdateById(UserUpdateDTO updateDTO)
        {
            var users = _iuser.UpdateById(updateDTO);

            if(users == null)
            {
                return NotFound("user not found");
            }
            else
            {
                return Ok(users);
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
        public ActionResult newUser(UserDTO user)
        {
            return Ok(_iuser.newUser(user));
        }
    }
}
