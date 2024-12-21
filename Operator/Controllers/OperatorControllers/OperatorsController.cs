using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Models;
using OP.DTO.Inbound;
using OP.Services.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class OperatorsController : ControllerBase
{
private readonly IOperatorService _operatorService;
    public OperatorsController(IOperatorService operatorService)
    {
            _operatorService = operatorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOperators(string? name, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var operators = await _operatorService.GetAllAsync(name: name, pageNumber : pageNumber, pageSize : pageSize);
            return Ok(operators);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOperatorById(int id)
    {
        try
        {
            var operatorEntity = await _operatorService.GetByIdAsync(id);
            if (operatorEntity == null)
                return NotFound();

                return Ok(operatorEntity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddOperator(CreateOperatorRequest operatorRequestDto)
    {
        try
        {
                var operatorEntity = operatorRequestDto.Adapt<Operator>();
                await _operatorService.CreateAsync(operatorEntity);
                return Created (nameof(GetOperatorById), new { id = operatorEntity.Id });
        }
        catch (Exception ex)
        {
                return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOperator(int id, [FromBody] CreateOperatorRequest operatorRequestDto)
    {
        try
            {
                await _operatorService.UpdateOperatorAsync(id, operatorRequestDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdateOperator(int id, [FromBody] JsonPatchDocument<CreateOperatorRequest> patchDoc)
        {
            try
            {
                await _operatorService.PatchOperatorAsync(id, patchDoc);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOperator(int id)
        {
            try
            {
                await _operatorService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
