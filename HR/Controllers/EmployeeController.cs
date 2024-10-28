using Common;
using HR.DTO;
using HR.DTO.Inbound;
using HR.DTO.outbound;

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

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        [HttpPost("addUser")]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableAddEmployees")]
        public async Task<ActionResult<ServiceResponse<NewEmployeeDto>>> AddUser([FromBody] NewEmployeeDto newEmployee)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);
                int userType = int.Parse(claimsPrincipal.FindFirst("userType")!.Value);
                DateOnly companyAnnualLeaveDate = DateOnly.Parse(claimsPrincipal.FindFirst("annualLeaveStartDate")!.Value);

               // await ManagerUserService.AddUser(newUser, myId, companyId, userType, companyAnnualLeaveDate);

                var serviceResponse = new ServiceResponse<NewEmployeeDto>(newEmployee, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }

    }
}
