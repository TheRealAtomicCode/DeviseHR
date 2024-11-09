using Microsoft.AspNetCore.Mvc;
using Models;
using OP.DTO;
using OP.Repository.Interfaces;

namespace OP.Controllers.OperatorControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorsController : ControllerBase
    {
        private readonly IGenericRepository<Operator> _repository;
        public OperatorsController(IGenericRepository<Operator> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<Operator>>> GetOperators(string? name, int? pageNumber = 1, int? pageSize = 10)
        {
            try
            {
                return Ok(await _repository.GetAllAsync(name: name, pageNumber: pageNumber ?? 1, pageSize: pageSize ?? 10));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
