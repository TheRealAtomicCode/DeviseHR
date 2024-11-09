using Mapster;
using Microsoft.AspNetCore.Mvc;
using Models;
using Common;
using OP.DTO;
using OP.DTO.Inbound;
using OP.Repository.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.JsonPatch;

namespace OP.Controllers.CompanyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IGenericRepository<Company> _companyRepository;
        private readonly IConfiguration _configuration;
        public CompanyController(IGenericRepository<Company> companyRepository, IConfiguration configuration)
        {
            _companyRepository = companyRepository;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Company>>>> GetCompanies(
            string? name, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _companyRepository.GetAllAsync(name, pageNumber, pageSize);
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
                var company = await _companyRepository.GetByIdAsync(id);
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

                if (operatorUserRole == 2 || operatorUserRole == 3)
                    return Unauthorized();

                var companyEntity = companyDto.Adapt<Company>();

                companyEntity.AddedByOperator = operatorId;

                await _companyRepository.AddAsync(companyEntity);

                return CreatedAtAction(nameof(GetCompanyById), new { id = companyEntity.Id }, companyDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateOperator([FromBody] JsonPatchDocument<CreateCompanyRequest> patchDoc, int id)
        {
            try
            {
                var companyEntity = await _companyRepository.GetByIdAsync(id);
                if (companyEntity == null)
                    return NotFound();

                var toPatch = companyEntity.Adapt<CreateCompanyRequest>();
                patchDoc.ApplyTo(toPatch);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                toPatch.Adapt(companyEntity);

                await _companyRepository.SaveChangesAsync();

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
                var company = await _companyRepository.GetByIdAsync(id);
                if (company == null)
                    return NotFound();
                
                await _companyRepository.DeleteAsync(company);
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
