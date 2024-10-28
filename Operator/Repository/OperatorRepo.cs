using Microsoft.EntityFrameworkCore;
using Models;
using OP.DTO;
using OP.Repository.Interfaces;

namespace OP.Repository
{
    public class OperatorRepo : IOperatorRepo
    {
        private readonly DeviseHrContext _Context;
        private readonly IConfiguration _configuration;

        public OperatorRepo(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _Context = dbContext;
            _configuration = configuration;
        }

        public async Task<Operator?> GetOperatorByEmail(string email)
        {
            return await _Context.Operators.FirstOrDefaultAsync(op => op.Email == email);
        }

        public async Task<Operator?> GetOperatorById(int id)
        {
            return await _Context.Operators.FirstOrDefaultAsync(op => op.Id == id);
        }

        public async Task<ServiceResponse<IEnumerable<Operator>>> GetAllOperators(string email, int pageNumber = 1, int pageSize = 10)
        {
            var collection = _Context.Operators.AsQueryable();

            if (!string.IsNullOrEmpty(email))
            {
                collection = collection.Where(op => op.Email.Contains(email));
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

        public async void IncrementLoginAttemt(Operator op)
        {
            op.LoginAttempt++;
            await _Context.SaveChangesAsync();
        }

        public async Task AddOperator(Operator op)
        {
            await _Context.Operators.AddAsync(op);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteOperator(Operator op)
        {
            _Context.Operators.Remove(op);
            await _Context.SaveChangesAsync();
        }
    }
}
