using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Models;
using HR.DTO;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Common;
using HR.Services.Interfaces;

namespace HR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ICredentialService _credentialService;
        private readonly IConfiguration _configuration;

        public AuthController(ICredentialService credentialService, IConfiguration configuration)
        {
            _credentialService = credentialService;
            _configuration = configuration;
        }



        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<LoginResponse>>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var empDto = await _credentialService.FindByCredentials(loginRequest);

                var sr = new ServiceResponse<LoginResponse>(empDto, true, "", 0);

                return Ok(sr);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<LoginResponse>(null!, false, ex.Message, 100001);
                return BadRequest(serviceResponse);
            }
        }



        [HttpPost("refresh")]
        [Authorize(Policy = "StaffMember")]
        public async Task<ActionResult<ServiceResponse<LoginResponse>>> Refresh([FromBody] RefreshRequestDto requestBody)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);
                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int userRole = int.Parse(claimsPrincipal.FindFirst("userRole")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);

                var empDto = await _credentialService.RefreshUserToken(myId, companyId, requestBody.RefreshToken);

                var sr = new ServiceResponse<LoginResponse>(empDto, true, "", 0);

                return Ok(sr);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<LoginResponse>(null!, false, ex.Message, 100002);
                return BadRequest(serviceResponse);
            }
        }


        [HttpDelete("logout")]
        [Authorize(Policy = "StaffMember")]
        public async Task<ActionResult<ServiceResponse<bool>>> Logout([FromBody] string refreshToken)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);
                int userId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);

                await _credentialService.LogoutService(userId, companyId, refreshToken);

                var serviceResponse = new ServiceResponse<bool>(true, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message, 100003);
                return BadRequest(serviceResponse);
            }
        }


        [HttpDelete("logoutAllDevices")]
        [Authorize(Policy = "StaffMember")]
        public async Task<ActionResult<ServiceResponse<bool>>> LogoutAllDevices()
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);
                int userId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);

                await _credentialService.LogoutService(userId, companyId, "");

                var serviceResponse = new ServiceResponse<bool>(true, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message, 100003);
                return BadRequest(serviceResponse);
            }
        }




        [HttpPatch("resetPassword")]
        public async Task<ActionResult<ServiceResponse<string>>> ResetPassword([FromBody] string email)
        {
            try
            {
                await _credentialService.ResetPasswordService(email.Trim(), false);

                var serviceResponse = new ServiceResponse<string>(email, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<string>("", false, ex.Message, 100004);
                return BadRequest(serviceResponse);
            }
        }



        [HttpPatch("confermResetPassword")]
        public async Task<ActionResult<ServiceResponse<LoginResponse>>> ConfermResetPassword([FromBody] ResetPasswordRequest requestBody)
        {
            try
            {
                var emp = await _credentialService.ConfirmVerificationCodeByEmail(requestBody.Email, requestBody.VerificationCode, requestBody.Password, requestBody.IsNewUser);

                var serviceResponse = new ServiceResponse<LoginResponse>(emp, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<LoginResponse>(null!, false, ex.Message, 100005);
                return BadRequest(serviceResponse);
            }
        }


    }
}
