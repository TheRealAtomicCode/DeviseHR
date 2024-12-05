using Microsoft.AspNetCore.Mvc;
using Models;
using OP.DTO;
using OP.DTO.Inbound;
using OP.DTO.Outbound;
using OP.Services.Interfaces;

namespace OP.Controllers.OperatorControllers
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
                var opDto = await _credentialService.FindByCredentials(loginRequest);

                var sr = new ServiceResponse<LoginResponse>(opDto, true, "", 0);

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
