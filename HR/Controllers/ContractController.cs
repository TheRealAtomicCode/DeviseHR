using Common;
using HR.DTO;
using HR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<ServiceResponse<AddContractRequest>>> CreateContract([FromBody] AddContractRequest newConract)
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

                var serviceResponse = new ServiceResponse<ContractDto>(addedContract, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<ContractDto>(null!, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }


        [HttpPost("CalculateLeaveYear")]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableAddEmployees")]
        public async Task<ActionResult<ServiceResponse<AddContractRequest>>> calculateLeaveYear([FromBody] AddContractRequest newConract)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);

                var calculatedContract = await _contractService.CalculateLeaveYear(newConract, companyId);

                var serviceResponse = new ServiceResponse<AddContractRequest>(calculatedContract, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<AddContractRequest>(null!, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }


        [HttpPost("GetLeaveYear/{employeeId}")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<ServiceResponse<LeaveYearResponse>>> GetLeaveYear([FromRoute] int employeeId, [Required] DateOnly leaveYearDate)
        {
            try
            {
                // NOTE!!!
                // this controller must take in the leaveYear Date based on the first annual leave year start date to get accurate results
                // for example: if the employees annual leave start date was 1st of april and the first contract started from 2022
                // then you should search from 01-04-2022 until 01-04-nextYear

                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);
                int myRole = int.Parse(claims.FindFirst("userRole")!.Value);

                var calculatedContract = await _contractService.GetLeaveYear(leaveYearDate, employeeId, myId, myRole, companyId);

                var serviceResponse = new ServiceResponse<LeaveYearResponse>(calculatedContract, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<LeaveYearResponse>(null!, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }


        [HttpPost("DetatchWorkingPattern/{contractId}")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<ServiceResponse<ContractDto>>> DetatchWorkingPattern([FromRoute] int contractId)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);
                int myRole = int.Parse(claims.FindFirst("userRole")!.Value);

                var detatchedContract = await _contractService.DetatchWorkingPattern(contractId, myId, myRole, companyId);

                var serviceResponse = new ServiceResponse<ContractDto>(detatchedContract, true, "", 0);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<LeaveYearResponse>(null!, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }



        [HttpPatch("{contractId}")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<ServiceResponse<bool>>> EditContract([FromRoute] int employeeId, [FromBody] JsonPatchDocument<EditContractRequest> patchDoc)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claims, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claims.FindFirst("id")!.Value);
                int companyId = int.Parse(claims.FindFirst("companyId")!.Value);
                int myRole = int.Parse(claims.FindFirst("userRole")!.Value);

                await _contractService.EditLastContract(patchDoc, employeeId, myId, myRole, companyId);

                var serviceResponse = new ServiceResponse<bool>(true, true, "", 0);

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
