using Common;
using HR.DTO;
using HR.DTO.Inbound;
using HR.DTO.outbound;
using HR.Services;
using HR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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



        [HttpPost("CreatePermission")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ServiceResponse<Permission>>> CreatePermission(PermissionData newPermission)
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


        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Permission>>>> GetAllPermission([FromQuery] int? page, [FromQuery] int? skip)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);

                List<Permission> permissions = await _permissionService.GetAllPermissions(companyId, page, skip);

                var sr = new ServiceResponse<List<Permission>>(permissions, true, "", 0);

                return Ok(sr);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<Permission>(null!, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }

        [HttpPatch("{permissionId}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ServiceResponse<NewEmployeeDto>>> EditPermission([FromRoute] int permissionId, [FromBody] JsonPatchDocument<EditPermissionDto> patchDoc)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);

                await _permissionService.EditPermission(patchDoc, permissionId, myId, companyId);

                return NoContent();
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }

        }


        [HttpGet("GetSubordinates/{managerId}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<SubordinateResponseDto>>>> GetSubordinates(int managerId)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);
                int myRole = int.Parse(claims.FindFirst("userRole")!.Value);

                List<SubordinateResponseDto> subordinates = await _permissionService.GetSubordinatesService(managerId, myId, companyId);

                var serviceResponse = new ServiceResponse<List<SubordinateResponseDto>>(subordinates, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<List<SubordinateResponseDto>>(null!, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }


        [HttpPatch("EditSubordinates")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ServiceResponse<string>>> EditSubordinates([FromRoute] int permissionId, [FromBody] EditSubordinatesDto editSubordinatesDto)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);
                int myRole = int.Parse(claims.FindFirst("userRole")!.Value);

                await _permissionService.EditSubordinatesService(editSubordinatesDto, myId, companyId);

                var serviceResponse = new ServiceResponse<string>("Subordinates Successfully edited", true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                string cleanedErrorMessage = ex.Message.Substring(ex.Message.IndexOf(":") + 2);
                var serviceResponse = new ServiceResponse<string>(null!, false, cleanedErrorMessage, 0);
                return BadRequest(serviceResponse);
            }
        }


    }
}
