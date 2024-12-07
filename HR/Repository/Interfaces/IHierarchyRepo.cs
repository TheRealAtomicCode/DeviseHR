using Models;

namespace HR.Repository.Interfaces
{
    public interface IHierarchyRepo
    {
        Task AddHierarchy(Hierarchy hierarchy);
        Task SaveChangesAsync();
    }
}