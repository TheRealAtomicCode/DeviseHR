﻿using Common;
using HR.DTO;
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
    public class WorkingPatternController : ControllerBase
    {
        private readonly IWorkingPatternService _workingPatternService;
        private readonly IConfiguration _configuration;

        public WorkingPatternController(IWorkingPatternService workingPatternService, IConfiguration configuration)
        {
            _workingPatternService = workingPatternService;
            _configuration = configuration;
        }


        [HttpPost]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableCreatePattern")]
        public async Task<ActionResult<ServiceResponse<WorkingPattern>>> CreateWorkingPattern([FromBody] WorkingPatternRequest workigPatternRequest)
        {
            try
            {
                string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
                Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);
                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int userRole = int.Parse(claimsPrincipal.FindFirst("userRole")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);

                var patternId = await _workingPatternService.AddWorkingPattern(workigPatternRequest, myId, companyId);

                return Created("Success", new { Id = patternId });
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<int?>(null!, false, ex.Message, 0);
                return BadRequest(serviceResponse);
            }
        }


    }
}