using AuthorizationAPI.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationAPI.Repository
{
    public class AuthenticationManager : IAuthenticationManager
    {
        AuditManagementSystemContext _context = new AuditManagementSystemContext();
        private readonly string tokenKey;
        public AuthenticationManager(string TokenKey)
        {
            
            this.tokenKey = TokenKey;
            

        }
        public AuthenticationManager()
        {

        }
        

        public AuthenticationManager(string TokenKey,AuditManagementSystemContext context)
        {
            this.tokenKey = TokenKey;
            _context = context;
        }


        public bool Authenticate(string email, string password)
        {
            if (!_context.Userdetails.Any(u => u.Email == email && u.Password == password))
            {
                return false;

            }

            return true;
        }     
        
        public int GetUserid(string email)
        {
            int userId =  _context.Userdetails.Single(user => user.Email == email).Userid;
            return userId;
        }

        public string GenerateAccessToken(Userdetails userInfo,IConfiguration _config)
        {
            if (userInfo == null)
                return null;
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    null,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public void assignRefreshToken(string refreshToken,string email)
        {
            Userdetails user = _context.Userdetails.Single(u => u.Email == email);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(2);
            _context.SaveChanges();
        }

        public Userdetails GetUser(string email)
        {
            return _context.Userdetails.Single(user => user.Email == email);
        }

        public string GetEmail(int id)
        {
            string email = _context.Userdetails.Single(user => user.Userid == id).Email;
            return email;
        }
    }
    }
