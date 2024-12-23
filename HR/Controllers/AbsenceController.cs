using Common;
using HR.DTO;
using HR.DTO.Inbound;
using HR.Services.Interfaces;
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
    public class AbsenceController : ControllerBase
    {
        private readonly IAbsenceService _absenceService;
        private readonly IConfiguration _configuration;

        public AbsenceController(IAbsenceService absenceService, IConfiguration configuration)
        {
            _absenceService = absenceService;
            _configuration = configuration;
        }

        [HttpPost("AddOrRequestAbsence")]
        [Authorize(Policy = "StaffMember")]
        public async Task<ActionResult<ServiceResponse<Absence>>> AddAbsence([FromBody] AddAbsenceRequest absenceRequest)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);
                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int userRole = int.Parse(claimsPrincipal.FindFirst("userRole")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);

                var addedAbsence = await _absenceService.AddOrRequestAbsence(absenceRequest, myId, companyId, userRole);

                var serviceResponse = new ServiceResponse<AbsenceDto>(addedAbsence, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                //var serviceResponse = new ServiceResponse<AddAbsenceRequest>(null!, false, ex.Message);
                //return BadRequest(serviceResponse);
                throw new NotImplementedException();
            }
        }
    }
}
