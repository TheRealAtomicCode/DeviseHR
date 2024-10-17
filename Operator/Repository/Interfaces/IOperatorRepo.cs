using Models;

namespace OP.Repository.Interfaces
{
    public interface IOperatorRepo
    {
        Task<Operator?> GetOperatorById(int id);
        Task<Operator?> GetOperatorByEmail(string email);
        Task<List<Operator>> GetAllOperators(string email);
        Task<Operator> AddOperator(Operator op);
        Task<Operator> UpdateOperator(Operator op);
        void IncrementLoginAttemt(Operator op);

    }
}
