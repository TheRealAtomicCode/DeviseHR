using HR.DTO;
using HR.DTO.Inbound;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace HR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<Employee>>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var db = new DeviseHrContext();

                Employee? employee = await db.Employees.Where(e => e.Email == loginRequest.Email).FirstOrDefaultAsync();

                if (employee == null) throw new Exception("Employee not found");

                var sr = new ServiceResponse<Employee>(employee);

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
