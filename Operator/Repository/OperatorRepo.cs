using Microsoft.EntityFrameworkCore;
using Models;
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
        public async Task<List<Operator>> GetAllOperators(string email)
        {
            return await _Context.Operators.ToListAsync();
        }

        public async void IncrementLoginAttemt(Operator op)
        {
            op.LoginAttempt++;
            await _Context.SaveChangesAsync();
        }

        public async Task<Operator> AddOperator(Operator op)
        {
            throw new NotImplementedException();
        }

        public async Task<Operator> DeleteOperator(Operator op)
        {
            throw new NotImplementedException();
        }

        public async Task<Operator> UpdateOperator(Operator op)
        {
            throw new NotImplementedException();
        }
    }
}
