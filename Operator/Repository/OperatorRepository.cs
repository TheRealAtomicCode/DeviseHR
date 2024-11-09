using Mapster;
using Microsoft.EntityFrameworkCore;
using Models;
using OP.DTO;
using OP.DTO.Outbound;
using OP.Repository.Interfaces;

namespace OP.Repository
{
    public class OperatorRepository : IGenericRepository<Operator>
    {
        private readonly DeviseHrContext _Context;
        private readonly IConfiguration _configuration;

        public OperatorRepository(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _Context = dbContext;
            _configuration = configuration;
        }

        public async Task<Operator?> GetByEmailAsync(string email)
        {
            return await _Context.Operators.FirstOrDefaultAsync(op => op.Email == email);
        }

        public async Task<Operator?> GetByIdAsync(int id)
        {
            var collection = await _Context.Operators.FirstOrDefaultAsync(op => op.Id == id);
            if(collection == null)
                return null;

            return collection;
        }

        public async Task<ServiceResponse<IEnumerable<Operator>>> GetAllAsync(string? name, int pageNumber = 1, int pageSize = 10)
        {
            var collection = _Context.Operators.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim();
                collection = collection.Where(op => op.GetFullName().Contains(name));
            }

            var totalItemCount = await collection.CountAsync();

            var collectionToReturn = await collection
                .OrderBy(c => c.CreatedAt)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
            
            return new ServiceResponse<IEnumerable<Operator>>(
                collectionToReturn,
                success: true,
                message: "Operators retrieved successfully",
                errorCode: 0,
                totalItemCount: totalItemCount,
                pageSize: pageSize,
                currentPage: pageNumber
            );
        }

        public async Task AddAsync(Operator op)
        {
            await _Context.Operators.AddAsync(op);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Operator op)
        {
            _Context.Operators.Remove(op);
            await _Context.SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _Context.SaveChangesAsync() >= 0;
        }
    }
}
