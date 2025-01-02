using Common;
using HR.DTO;
using HR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HR.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IConfiguration configuration, IEmployeeService employeeService)
        {
            _configuration = configuration;
            _employeeService = employeeService;
        }


        [HttpPost("createEmployee")]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableAddEmployees")]
        public async Task<IActionResult> CreateEmployee([FromBody] NewEmployeeRequest newEmployee)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);
                int myRole = int.Parse(claims.FindFirst("userRole")!.Value);

                DateOnly companyAnnualLeaveDate = DateOnly.Parse(claims.FindFirst("annualLeaveStartDate")!.Value);

                int employeeId = await _employeeService.CreateEmployee(newEmployee, myId, companyId, myRole);

                return Created("Success", new { Id = employeeId });
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
            
        }


        [HttpGet("{employeeId}")]
        [Authorize(Policy = "StaffMember")]
        public async Task<ActionResult<ServiceResponse<EmployeeDto>>> GetEmployee([FromRoute] int employeeId)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);
                int myRole = int.Parse(claims.FindFirst("userRole")!.Value);

                EmployeeDto employeeDto = await _employeeService.GetEmployee(employeeId, myId, companyId, myRole);

                var sr = new ServiceResponse<EmployeeDto>(employeeDto, true, "", 0);

                return Ok(sr);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }

        }


        [HttpGet]
        [Authorize(Policy = "StaffMember")]
        public async Task<ActionResult<ServiceResponse<List<FoundEmployee>>>> GetAllEmployee([FromQuery] string? searchTerm,[FromQuery] int? page, [FromQuery] int? skip)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);
                int myRole = int.Parse(claims.FindFirst("userRole")!.Value);
                bool enableShowEmployees = bool.Parse(claims.FindFirst("enableShowEmployees")!.Value);

                List<FoundEmployee> foundEmployees = await _employeeService.GetAllEmployees(searchTerm, page, skip, myId, companyId, myRole, enableShowEmployees);

                var sr = new ServiceResponse<List<FoundEmployee>>(foundEmployees, true, "", 0);

                return Ok(sr);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }


        [HttpPatch("{employeeId}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ServiceResponse<NewEmployeeRequest>>> EditEmployee([FromRoute] int employeeId, [FromBody] JsonPatchDocument<EditEmployeeRequest>  patchDoc)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);

                await _employeeService.EditEmployee(patchDoc, employeeId, myId, companyId);

                return NoContent();
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }

        }




        //[HttpDelete("{employeeId}")]
        //[Authorize(Policy = "Admin")]
        //public async Task<ActionResult<ServiceResponse<NewEmployeeDto>>> DeleteEmployee([FromRoute] int employeeId)
        //{
        //    try
        //    {
        //        string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
        //        Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

        //        int myId = int.Parse(claims.FindFirst("id")!.Value);
        //        int companyId = int.Parse(claims.FindFirst("companyId")!.Value);

        //        int employeeId = await _employeeService.DeleteEmployee(employeeId, myId, companyId);

        //        var sr = new ServiceResponse<List<FoundEmployee>>(foundEmployees, true, "", 0);

        //        return Ok(sr);
        //    }
        //    catch (Exception ex)
        //    {
        //        var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message, 0);
        //        return BadRequest(serviceResponse);
        //    }

        //}



    }
}
