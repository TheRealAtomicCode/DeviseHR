using Microsoft.AspNetCore.JsonPatch;
using Models;
using OP.DTO.Inbound;

namespace OP.Services.Interfaces
{
    public interface IOperatorService
    {
        Task<List<Operator>> GetAllAsync(string? name, int pageNumber = 1, int pageSize = 10);
        Task<Operator?> GetByIdAsync(int id);
        Task CreateAsync(Operator operatorEntity);
        Task UpdateOperatorAsync(int id, CreateOperatorRequest operatorDto);
        Task PatchOperatorAsync(int id, JsonPatchDocument<CreateOperatorRequest> patchDoc);
        Task DeleteAsync(int id);
    }

    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetCompaniesAsync(string? name, int pageNumber, int pageSize);
        Task<Company> GetCompanyByIdAsync(int id);
        Task AddCompanyAsync(CreateCompanyRequest companyDto, int operatorId);
        Task PatchCompanyAsync(int id, JsonPatchDocument<CreateCompanyRequest> patchDoc);
        Task DeleteCompanyAsync(int id);
    }
}
