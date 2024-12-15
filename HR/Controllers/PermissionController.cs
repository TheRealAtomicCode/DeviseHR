using Common;
using HR.DTO;
using HR.DTO.Inbound;
using HR.Services;
using HR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IPermissionService _permissionService;

        public PermissionController(IConfiguration configuration, IPermissionService permissionService)
        {
            _configuration = configuration;
            _permissionService = permissionService;
        }



        [HttpPost("CreateRole")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ServiceResponse<Permission>>> CreateRole(PermissionData newPermission)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);
                int myRole = int.Parse(claims.FindFirst("userRole")!.Value);

                int permissionId = await _permissionService.CreatePermissionService(newPermission, myId, companyId);

                return Created("Success", new { Id = permissionId });
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<Permission>(null!, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }


    }
}
