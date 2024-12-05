using Models;
using Microsoft.AspNetCore.JsonPatch;
using Op.Repository.IRepostiory;
using OP.Services.Interfaces;
using OP.DTO.Inbound;
using Mapster;
using System.Linq.Expressions;

namespace OP.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IGenericRepository<Company> _repository;

        public CompanyService(IGenericRepository<Company> companyRepository)
        {
            _repository = companyRepository;
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(string? name, int pageNumber, int pageSize)
        {
            Expression<Func<Company, bool>>? filter = string.IsNullOrEmpty(name)
                ? null
                : c => c.CompanyName.Contains(name);

            return await _repository.GetAllAsync(
                filter: filter,
                pageSize: pageSize,
                pageNumber: pageNumber
            );
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await _repository.GetAsync(
                filter: c => c.Id == id,
                tracked: true
            );
        }

        public async Task AddCompanyAsync(CreateCompanyRequest companyDto, int operatorId)
        {
            var companyEntity = companyDto.Adapt<Company>();
            companyEntity.AddedByOperator = operatorId;

            await _repository.CreateAsync(companyEntity);
        }

        public async Task PatchCompanyAsync(int id, JsonPatchDocument<CreateCompanyRequest> patchDoc)
        {
            var companyEntity = await _repository.GetAsync(filter: c => c.Id == id);
            if (companyEntity == null)
                throw new Exception($"Company with ID {id} not found.");

            var toPatch = companyEntity.Adapt<CreateCompanyRequest>();
            patchDoc.ApplyTo(toPatch);

            toPatch.Adapt(companyEntity);

            await _repository.SaveAsync();
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var companyEntity = await _repository.GetAsync(filter: c => c.Id == id);
            if (companyEntity == null)
                throw new Exception($"Company with ID {id} not found.");

            await _repository.RemoveAsync(companyEntity);
        }
    }
}
