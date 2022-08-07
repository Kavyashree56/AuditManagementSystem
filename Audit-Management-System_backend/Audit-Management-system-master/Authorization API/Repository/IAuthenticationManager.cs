using AuthorizationAPI.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationAPI.Repository
{
    public interface IAuthenticationManager
    {
        public bool Authenticate(string email, string password);
        int GetUserid(string email);
        Userdetails GetUser(string email);
        string GenerateAccessToken(Userdetails userInfo, IConfiguration config);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        void assignRefreshToken(string refreshToken,string email);

        string GetEmail(int userId);



    }
}
