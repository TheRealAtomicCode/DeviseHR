using Common;
using HR.DTO;
using HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingPatternController : ControllerBase
    {
        private readonly WorkingPatternService _workingPatternService;
        private readonly IConfiguration _configuration;

        public WorkingPatternController(WorkingPatternService workingPatternService, IConfiguration configuration)
        {
            _workingPatternService = workingPatternService;
            _configuration = configuration;
        }


        [HttpPost]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableCreatePattern")]
        public async Task<ActionResult<ServiceResponse<WorkingPattern>>> CreateWorkingPattern([FromBody] WorkingPatternRequest workigPatternRequest)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);
                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int userRole = int.Parse(claimsPrincipal.FindFirst("userRole")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);

                var patternId = await _workingPatternService.AddWorkingPattern(workigPatternRequest, myId, companyId);

                return Created("Success", new { Id = patternId });
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<int?>(null!, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }



        [HttpGet]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableAddEmployees")]
        public async Task<ActionResult<ServiceResponse<List<FoundEmployee>>>> GetAllEmployee([FromQuery] string? searchTerm, [FromQuery] int? page, [FromQuery] int? skip)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);
                int myRole = int.Parse(claims.FindFirst("userRole")!.Value);
                bool enableShowEmployees = bool.Parse(claims.FindFirst("enableShowEmployees")!.Value);

                var foundWorkingPatterns = await _workingPatternService.GetAllWorkingPatterns(searchTerm, page, skip, companyId);

                var sr = new ServiceResponse<List<WorkingPatternDto>>(foundWorkingPatterns, true, "", 0);

                return Ok(sr);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }

        }


        [HttpGet("GetWorkingPatternEmployees/{workingPatternId}")]
        [Authorize(Policy = "Manager")]

        public async Task<ActionResult<ServiceResponse<List<EmployeeWithContractDto>>>> GetWorkingPatternEmployees([FromRoute] int workingPatternId, [FromQuery] string? searchTerm, [FromQuery] int? page, [FromQuery] int? skip)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);
                int myRole = int.Parse(claims.FindFirst("userRole")!.Value);
                bool enableShowEmployees = bool.Parse(claims.FindFirst("enableShowEmployees")!.Value);

                var employeesWithContracts = await _workingPatternService.GetWorkingPatternEmployees(workingPatternId, searchTerm, page, skip, myId, companyId, myRole, enableShowEmployees);

                var sr = new ServiceResponse<List<EmployeeWithContractDto>>(employeesWithContracts, true, "", 0);

                return Ok(sr);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }

        }


    }
}
