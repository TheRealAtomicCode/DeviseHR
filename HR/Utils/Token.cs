﻿using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HR.Utils
{
    public static class Token
    {

        public static async Task<string> GenerateEmployeeJWT(Employee emp, string secret, string jwtExpTime)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>();

            claims = new List<Claim>
                {
                    new Claim("id", emp.Id.ToString()),
                    new Claim("userRole", emp.UserRole.ToString()),
                };

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
        public static void ExtractClaimsFromToken(string clientJwt, string jwtSecret, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtSecurityToken)
        {
            // Validate Token
            var tokenHandler = new JwtSecurityTokenHandler();

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