using Models;
using OP.DTO;

namespace OP.Repository.Interfaces
{
    public interface IOperatorRepo
    {
        Task<Operator?> GetOperatorById(int id);
        Task<Operator?> GetOperatorByEmail(string email);
        Task<ServiceResponse<IEnumerable<Operator>>> GetAllOperators(string email, int pageNumber = 1, int pageSize = 10);
        Task AddOperator(Operator op);
        Task DeleteOperator(Operator op);
        void IncrementLoginAttemt(Operator op);
    }
}
