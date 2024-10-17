using Microsoft.EntityFrameworkCore;
using Models;
using OP.Repository.Interfaces;

namespace OP.Repository
{
    public class OperatorRepo : IOperatorRepo
    {

        private readonly DeviseHrContext _dbContext;

        public OperatorRepo(DeviseHrContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Operator> AddOperator(Operator op)
        {
            throw new NotImplementedException();
        }

        public async Task<Operator> DeleteOperator(Operator op)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Operator>> GetAllOperators(string email)
        {
            return await _dbContext.Operators.ToListAsync();
        }

        public async Task<Operator?> GetOperatorByEmail(string email)
        {
            return _dbContext.Operators.FirstOrDefault(op => op.Email == email);
        }

        public async Task<Operator?> GetOperatorById(int id)
        {
            return await _dbContext.Operators.FirstOrDefaultAsync(op => op.Id == id);
        }

        public async Task<Operator> UpdateOperator(Operator op)
        {
            throw new NotImplementedException();
        }
    }
}
