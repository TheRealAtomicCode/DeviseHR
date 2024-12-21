using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Models;
using OP.DTO;
using OP.DTO.Inbound;
using OP.Repository;
using OP.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _service;
    private readonly IConfiguration _configuration;

    public CompanyController(ICompanyService companyService, IConfiguration configuration)
    {
        _service = companyService;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<IEnumerable<Company>>>> GetCompanies(
        string? name, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var result = await _service.GetCompaniesAsync(name, pageNumber, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Company>> GetCompanyById(int id)
    {
        try
        {
            var company = await _service.GetCompanyByIdAsync(id);
            if (company == null)
                return NotFound();

            return Ok(company);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult> CreateCompany([FromBody] CreateCompanyRequest companyDto)
    {
        try
        {
            if (companyDto == null)
                return BadRequest();

            string clientJWT = Token.ExtractTokenFromRequestHeaders(HttpContext);
            Token.ExtractClaimsFromToken(clientJWT, _configuration, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

            int operatorId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
            int operatorUserRole = int.Parse(claimsPrincipal.FindFirst("userRole")!.Value);

            if (operatorUserRole <= StaticRoles.Manager)
                return Unauthorized();

            await _service.AddCompanyAsync(companyDto, operatorId);

            return Created(nameof(GetCompanyById), companyDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateCompany(int id, [FromBody] JsonPatchDocument<CreateCompanyRequest> patchDoc)
    {
        try
        {
            await _service.PatchCompanyAsync(id, patchDoc);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        try
        {
            await _service.DeleteCompanyAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}