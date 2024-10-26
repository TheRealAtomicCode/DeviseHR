using HR.DTO;
using HR.DTO.Inbound;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

// branch test  2

namespace HR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        [HttpPost("login")]
        public ActionResult<ServiceResponse<Employee>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var sr = new ServiceResponse<LoginRequest>(loginRequest);

                return Ok(sr);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<LoginRequest>(null!, false, ex.Message, 000001);
                return BadRequest(serviceResponse);
            }
        }


    }
}
