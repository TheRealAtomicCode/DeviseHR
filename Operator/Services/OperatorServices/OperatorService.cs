using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using Models;
using Op.Repository.IRepostiory;
using OP.DTO.Inbound;
using OP.Services.Interfaces;

namespace OP.Services.OperatorServices
{
    public class OperatorService : IOperatorService
    {
        private readonly IGenericRepository<Operator> _repository;

        public OperatorService(IGenericRepository<Operator> repository)
        {
            _repository = repository;
        }

        public async Task<List<Operator>> GetAllAsync(string? name, int pageNumber = 1, int pageSize = 10)
        {
            return await _repository.GetAllAsync(
                filter: string.IsNullOrWhiteSpace(name) ? null : op => op.GetFullName().Contains(name),
                pageNumber: pageNumber,
                pageSize: pageSize
            );
        }

        public async Task<Operator?> GetByIdAsync(int id)
        {
            return await _repository.GetAsync(op => op.Id == id);
        }

        public async Task CreateAsync(Operator operatorEntity)
        {
            await _repository.CreateAsync(operatorEntity);
        }

        public async Task UpdateOperatorAsync(int id, CreateOperatorRequest operatorDto)
        {
            var operatorEntity = await _repository.GetAsync(op => op.Id == id,false);
            if (operatorEntity == null) throw new Exception($"Operator with id {id} not found.");

            var op = operatorDto.Adapt(operatorEntity);
            await _repository.UpdateAsync(op);
        }

        public async Task PatchOperatorAsync(int id, JsonPatchDocument<CreateOperatorRequest> patchDoc)
        {
            var operatorEntity = await _repository.GetAsync(op => op.Id == id);
            if (operatorEntity == null) throw new Exception($"Operator with id {id} not found.");

            var operatorToPatch = operatorEntity.Adapt<CreateOperatorRequest>();
            patchDoc.ApplyTo(operatorToPatch);

            operatorToPatch.Adapt(operatorEntity);
            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var operatorEntity = await _repository.GetAsync(op => op.Id == id);
            if (operatorEntity == null) throw new Exception("Operator not found");

            await _repository.RemoveAsync(operatorEntity);
        }
    }
}
