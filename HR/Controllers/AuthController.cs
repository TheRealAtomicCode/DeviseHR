using HR.DTO;
using HR.DTO.Inbound;
using HR.DTO.Outbound;
using HR.Services.EmployeeServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

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
                var empDto = await _credentialService.FindByCredentialts(loginRequest);

                var sr = new ServiceResponse<LoginResponse>(empDto, true, "", 0);

                return Ok(sr);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<Employee>(null!, false, ex.Message, 000001);
                return BadRequest(serviceResponse);
            }
        }
    
    
    }
}
