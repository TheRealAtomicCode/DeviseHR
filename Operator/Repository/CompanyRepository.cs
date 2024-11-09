using Microsoft.EntityFrameworkCore;
using Models;
using OP.DTO;
using OP.Repository.Interfaces;

namespace OP.Repository
{
    public class CompanyRepository : IGenericRepository<Company>
    {
        private readonly DeviseHrContext _context;
        public CompanyRepository(DeviseHrContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Company opj)
        {
            await _context.Companies.AddAsync(opj);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Company opj)
        {
            _context.Companies.Remove(opj);
            await _context.SaveChangesAsync();
        }

        public async Task<ServiceResponse<IEnumerable<Company>>> GetAllAsync(string? name, int pageNumber = 1, int pageSize = 10)
        {
            var collection = _context.Companies.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim();
                collection = collection.Where(op => op.CompanyName.Contains(name));
            }

            var totalItemCount = await collection.CountAsync();

            var collectionToReturn = await collection
                .OrderBy(c => c.CreatedAt)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return new ServiceResponse<IEnumerable<Company>>(
                collectionToReturn,
                success: true,
                message: "Companies retrieved successfully",
                errorCode: 0,
                totalItemCount: totalItemCount,
                pageSize: pageSize,
                currentPage: pageNumber
            );
        }

        public Task<Company?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            var op = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if(op == null)
                return null;

            return op;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
