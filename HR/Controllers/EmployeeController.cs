using Common;
using HR.DTO;
using HR.DTO.Inbound;
using HR.DTO.outbound;

using HR.Services.UserServices;
using HR.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<ServiceResponse<NewEmployeeDto>>> CreateEmployee([FromBody] NewEmployeeDto newEmployee)
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



    }
}
