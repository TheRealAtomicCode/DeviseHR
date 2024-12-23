using HR.DTO;
using HR.DTO.Inbound;
using HR.DTO.outbound;
using Models;

namespace HR.Repository.Interfaces
{
    public interface IHierarchyRepo
    {
       // Task AddHierarchy(Hierarchy hierarchy);
        Task RemoveHierarchy(int managerId, int subordinateId);
        Task AddHierarchy(int managerId, int subordinateId);

        Task<bool> HasManager(int subordinateId);
        Task<bool> IsRelated(int managerId, int subordinateId);


        Task<List<NonManagerUserDto>> GetNoneManagerEmployeesByIdList(List<int> employeeIdList, int companyId);
        Task<List<SubordinateResponseDto>> GetSubordinatesByManagerId(int managerId, int companyId);

        // sub repos
        Task<int> ValidateRequestOrAddAbsence(int myId, int userRole, int EmployeeId);

    }
}
