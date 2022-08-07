using AuthorizationAPI.Model;
using AuthorizationAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthenticationManager manager;
        private IConfiguration config;

        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(LoginController));
        Userdetails _user = new Userdetails();
        public LoginController(IAuthenticationManager manager, IConfiguration config)
        {
            this.manager = manager;
            this.config = config;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello";
        }

        [AllowAnonymous]
        [HttpPost("AuthenicateUser")]
        public IActionResult AuthenticateUser([FromBody] Userdetails details)
        {
            _log4net.Info("Http Authentication Login request Initiated");


            var isValid = manager.Authenticate(details.Email, details.Password);
            if (!isValid)
                return Unauthorized();
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, details.Email)
                                    };
            var token = manager.GenerateAccessToken(details,config);
            var refreshToken = manager.GenerateRefreshToken();

            manager.assignRefreshToken(refreshToken,details.Email);
            LoginResponse response = new LoginResponse
            {
                UserId = manager.GetUserid(details.Email),
                Email = details.Email,
                AccessToken = token,
                RefreshToken = refreshToken
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(LoginViewModel loginResponse)
        {
            if (loginResponse is null)
                return BadRequest("Invalid client request");
            string accessToken = loginResponse.AccessToken;
            string refreshToken = loginResponse.RefreshToken;
            string email = loginResponse.Email;
            var principal = manager.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default
            var user = manager.GetUser(email);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");
            var newAccessToken = manager.GenerateAccessToken(user,config);
            var newRefreshToken = manager.GenerateRefreshToken();
            manager.assignRefreshToken(newRefreshToken, email);
            return Ok(new LoginResponse()
            {
                UserId = manager.GetUserid(email),
                Email = email,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var email = User.Identity.Name;
            var user = manager.GetUser(email);
            if (user == null) return BadRequest();
            manager.assignRefreshToken(null, email);
            return NoContent();
        }

    }
}
