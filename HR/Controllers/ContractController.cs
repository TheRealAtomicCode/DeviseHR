using Common;
using HR.DTO;
using HR.DTO.Inbound;
using HR.DTO.outbound;
using HR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using System.ComponentModel.DataAnnotations;
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

                var addedContract = await _contractService.CreateContract(newConract, myId, companyId, myRole);

                var serviceResponse = new ServiceResponse<Contract>(addedContract, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<CreateContractDto>(null!, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }


        [HttpPost("CalculateLeaveYear")]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableAddEmployees")]
        public async Task<ActionResult<ServiceResponse<CreateContractDto>>> calculateLeaveYear([FromBody] CreateContractDto newConract)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);

                var calculatedContract = await _contractService.CalculateLeaveYear(newConract, companyId);

                var serviceResponse = new ServiceResponse<CreateContractDto>(calculatedContract, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<CreateContractDto>(null!, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }


        [HttpPost("GetLeaveYear/{employeeId}")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<ServiceResponse<ContractAndLeaveYearCount>>> GetLeaveYear([FromRoute] int employeeId, [Required] DateOnly leaveYearDate)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);
                int myRole = int.Parse(claims.FindFirst("userRole")!.Value);

                var calculatedContract = await _contractService.GetLeaveYear(leaveYearDate, employeeId, myId, myRole, companyId);

                var serviceResponse = new ServiceResponse<ContractAndLeaveYearCount>(calculatedContract, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<ContractAndLeaveYearCount>(null!, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }


    }
}
