using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;

namespace OP.Subroutines
{
    public static class Token
    {

        

        //public static async Task<string> GenerateUserAuthJWT(Operator operator)
        //{
        //    string? jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");

        //    if (string.IsNullOrWhiteSpace(jwtSecret)) throw new Exception("Environment Error");

        //    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret));

        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //    var claims = GenerateUserJwtClaims(user);

        //    string? jwtExpTime = Environment.GetEnvironmentVariable("EXPTIME");

        //    if (string.IsNullOrWhiteSpace(jwtExpTime)) throw new Exception("Environment Error");

        //    var token = new JwtSecurityToken(
        //        expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtExpTime)),
        //        signingCredentials: credentials,
        //        claims: claims // Add the claims to the token
        //    );

        //    var jwt = await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));

        //    return jwt;
        //}

    }
}
