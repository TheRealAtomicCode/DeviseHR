using Common;
using HR.DTO;
using HR.DTO.Inbound;
using HR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IContractService _contractService;

        public ContractController(IConfiguration configuration, IContractService contractService)
        {
            _configuration = configuration;
            _contractService = contractService;
        }


        [HttpPost("CreateContract")]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableAddEmployees")]
        public async Task<ActionResult<ServiceResponse<CreateContractDto>>> CreateContract([FromBody] CreateContractDto newConract)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);
                int myRole = int.Parse(claims.FindFirst("userRole")!.Value);

                DateOnly companyAnnualLeaveDate = DateOnly.Parse(claims.FindFirst("annualLeaveStartDate")!.Value);
                bool enableAddEmployees = bool.Parse(claims.FindFirst("enableAddEmployees")!.Value);

              //  var calculatedContract = await ManagerContractService.AddContract(newConract, myId, companyId, myRole, enableAddEmployees);

              //  var serviceResponse = new ServiceResponse<Contract>(calculatedContract, true, "");

               // return Ok(serviceResponse);

               throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<CreateContractDto>(null!, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }


    }
}
