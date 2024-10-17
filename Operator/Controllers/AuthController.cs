using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Models;
using OP.DTO;
using OP.DTO.Inbound;
using OP.Services.OperatorService;
using OP.Services.OperatorService.Interfaces;

namespace OP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ICredentialService _credentialService;
        public AuthController(ICredentialService credentialService) 
        {
            _credentialService = credentialService;
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {

                var op = await _credentialService.FindByCredentialts(loginRequest);


                var sr = new ServiceResponse<Operator>(op);

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
