//using EFExample.Models;
//using EFExample.Secutiy;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace EFExample.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class OauthController : ControllerBase
//    {
//        public readonly SocialMediaContext _socialMediaContext;
//        public readonly JwtToken _jwtToken;
//        public OauthController(SocialMediaContext socialMediaContext, JwtToken jwtToken)
//        {
//            _socialMediaContext = socialMediaContext;
//            _jwtToken = jwtToken;
//        }

//        [HttpGet("login")]
//        public IActionResult Login()
//        {
//            var properties = new AuthenticationProperties
//            {
//                RedirectUri = Url.Action(nameof(Callback), "Oauth"),
//                Items = { { "scheme", "Google" } }
//            };

//            return Challenge(properties, "Google");
//        }

//        [HttpGet("Callback")]
//        public async Task<IActionResult> Callback()
//        {
//            var authResult = await HttpContext.AuthenticateAsync("Google");

//            if (!authResult.Succeeded)
//            {
//                return Unauthorized("OAuth authentication failed.");
//            }

//            var userEmail = authResult.Principal.FindFirst(ClaimTypes.Email)?.Value;

//            var authorizedEmails = _socialMediaContext.Users.FirstOrDefault(e => e.Email.Equals(userEmail));

//            if (authorizedEmails != null)
//            {
//                var jwtToken = _jwtToken.GenerateJwtToken(authorizedEmails);

//                return Ok(new { Token = jwtToken });
//            }
//            else
//            {
//                return Unauthorized("User not authorized.");
//            }
//        }
//    }
//}


//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.OAuth;
//using Microsoft.AspNetCore.Authentication;
//using System.Security.Claims;
///// <summary>
///// Grant resource owner credentials overload method.
///// </summary>
///// <param name="context">Context parameter</param>
///// <returns>Returns when task is completed</returns>
//public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
//{
//    // Initialization.
//    string usernameVal = context.UserName;
//    string passwordVal = context.Password;
//    var user = this.databaseManager.LoginByUsernamePassword(usernameVal, passwordVal).ToList();

//    // Verification.
//    if (user == null || user.Count() <= 0)
//    {
//        // Settings.
//        context.SetError("invalid_grant", "The user name or password is incorrect.");

//        // Retuen info.
//        return;
//    }

//    // Initialization.
//    var claims = new List<Claim>();
//    var userInfo = user.FirstOrDefault();

//    // Setting
//    claims.Add(new Claim(ClaimTypes.Name, userInfo.username));

//    // Setting Claim Identities for OAUTH 2 protocol.
//    ClaimsIdentity oAuthClaimIdentity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
//    ClaimsIdentity cookiesClaimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);

//    // Setting user authentication.
//    AuthenticationProperties properties = CreateProperties(userInfo.username);
//    AuthenticationTicket ticket = new AuthenticationTicket(oAuthClaimIdentity, properties);

//    // Grant access to authorize user.
//    context.Validated(ticket);
//    context.Request.Context.Authentication.SignIn(cookiesClaimIdentity);
//}

//#endregion