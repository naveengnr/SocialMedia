﻿using EFExample.DTO;
using EFExample.Models;
using EFExample.Secutiy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EFExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController:ControllerBase
    {
        public readonly SocialMediaContext _context;
        public IConfiguration _configuration;

        // public readonly JwtConfig _jwtConfig;

        public AuthenticationController(SocialMediaContext context, IConfiguration configuration)
        {
            _context = context;
            //_jwtConfig = jwtConfig;
            _configuration = configuration;
        }

        [HttpPost("Authentication")]
        public IActionResult Login(AuthenticationDTO authenticationDTO)
        {

            var Exists = _context.Users.FirstOrDefault(e => e.Username.Equals(authenticationDTO.Username));

            if(Exists == null)
            {
                return BadRequest("Username is wrong");
            }

            var password = Dcrypt.DecryptPassword(Exists.Password);

            if (Exists != null && password.Equals(authenticationDTO.Password))
            {
                var token = GenerateJwtToken(Exists);

                return Ok(new AuthResult()
                {
                    Token = token,
                    Result = true
                });
            }
            else
            {

                return BadRequest(new AuthResult()
                {

                    error = new List<string>
                {
                    " wrong usernme or password"
                },
                    Result = false


                });

            }
        }
        private string GenerateJwtToken (User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),

                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key) , SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return  jwtTokenHandler.WriteToken(token);

        }
    }
}
