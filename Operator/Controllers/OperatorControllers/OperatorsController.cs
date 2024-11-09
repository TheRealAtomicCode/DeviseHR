using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Models;
using OP.DTO;
using OP.DTO.Inbound;
using OP.DTO.Outbound;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Operator>>> GetOperatorById(int id)
        {
            try
            {
                var operatorEntity = await _repository.GetByIdAsync(id);
                if (operatorEntity == null)
                    return NotFound();

                var operatorDto = operatorEntity.Adapt<OperatorResponseDto>();
                return Ok(operatorDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddOperator(CreateOperatorRequest operatorRequestDto)
        {
            try
            {
                if (operatorRequestDto == null)
                    return BadRequest();

                Operator operatorObj = operatorRequestDto.Adapt<Operator>();
                operatorObj.CreatedAt = DateTime.Now;

                await _repository.AddAsync(operatorObj);
                
                return CreatedAtAction(nameof(GetOperatorById), new { Id = operatorObj.Id }, operatorRequestDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateOperator([FromBody] JsonPatchDocument<CreateOperatorRequest> patchDoc, int id)
        {
            try
            {
                var operatorEntity = await _repository.GetByIdAsync(id);
                if (operatorEntity == null)
                    return NotFound();

                var toPatch = operatorEntity.Adapt<CreateOperatorRequest>();
                patchDoc.ApplyTo(toPatch);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                toPatch.Adapt(operatorEntity);

                await _repository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DelateOperator(int id)
        {
            try
            {
                var operatorEntity = await _repository.GetByIdAsync(id);
                if (operatorEntity == null)
                    return NotFound();

                await _repository.DeleteAsync(operatorEntity);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
