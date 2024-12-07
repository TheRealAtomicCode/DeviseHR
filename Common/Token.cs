using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Token
    {
        public static async Task<string> GenerateJWT(IConfiguration _configuration, string tokenType, List<Claim> claims)
        {
            

            string jwtSecret = _configuration[$"{tokenType}:SecretKey"]!;
            string jwtExpTime = _configuration[$"{tokenType}:ExpTime"]!;

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            
            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtExpTime)),
                signingCredentials: credentials,
                claims: claims // Add the claims to the token
            );

            var jwt = await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));
            return jwt;
        }

        public static string ExtractTokenFromRequestHeaders(HttpContext httpContext)
        {
            string? token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null) throw new Exception("Failed to Authenticate Request headers");

            return token;
        }

        // Extract claims 
        public static void ExtractClaimsFromToken(string clientJwt, IConfiguration _configuration, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtSecurityToken)
        {
            // Validate Token
            var tokenHandler = new JwtSecurityTokenHandler();

            string jwtSecret = _configuration["token:SecretKey"]!;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret))
            };

            SecurityToken validateToken;
            claimsPrincipal = tokenHandler.ValidateToken(clientJwt, tokenValidationParameters, out validateToken);

            jwtSecurityToken = (JwtSecurityToken)validateToken;
        }


    }
}
