using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Models;
using HR.DTO;
using HR.DTO.Inbound;
using HR.DTO.Outbound;
using HR.Services.EmployeeService;
using HR.Services.EmployeeService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HR.Utils;

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
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult<ServiceResponse<LoginResponse>>> Refresh([FromBody] string refreshToken)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                string jwtSecret = _configuration["JwtSettings:SecretKey"]!;
                Token.ExtractClaimsFromToken(clientJWT, jwtSecret, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int userType = int.Parse(claimsPrincipal.FindFirst("userRole")!.Value);

                var empDto = await _credentialService.RefreshUserToken(myId, refreshToken);

                var sr = new ServiceResponse<LoginResponse>(empDto, true, "", 0);

                return Ok(sr);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<LoginResponse>(null!, false, ex.Message, 100002);
                return BadRequest(serviceResponse);
            }
        }


    }
}
