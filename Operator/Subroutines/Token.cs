using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OP.Subroutines
{
    public static class Token
    {

        public static async Task<string> GenerateOperatorJWT(Operator op, string secret, string jwtExpTime)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>();

            claims = new List<Claim>
                {
                    new Claim("id", op.Id.ToString()),
                    new Claim("userRole", op.UserRole.ToString()),
                };

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtExpTime)),
                signingCredentials: credentials,
                claims: claims // Add the claims to the token
            );

            var jwt = await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));

            return jwt;
        }




    }
}
