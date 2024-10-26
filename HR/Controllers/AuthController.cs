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
using HR.Subroutines;

namespace HR.Controllers.OperatorControllers
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
                var empDto = await _credentialService.FindByCredentialts(loginRequest);

                var sr = new ServiceResponse<LoginResponse>(empDto, true, "", 0);

                return Ok(sr);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<Employee>(null!, false, ex.Message, 100001);
                return BadRequest(serviceResponse);
            }
        }


    }
}
