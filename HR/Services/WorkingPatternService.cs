using HR.DTO;
using HR.Repository;
using HR.Repository.Interfaces;
using HR.Services.Interfaces;
using HR.UOW;
using HR.UOW.Interfaces;
using Mapster;
using Models;

namespace HR.Services
{
    public class WorkingPatternService : IWorkingPatternService
    {

        private readonly IMainUOW _mainUOW;
        private readonly IConfiguration _configuration;

        public WorkingPatternService(IMainUOW mainUOW, IConfiguration configuration)
        {
            _mainUOW = mainUOW;
            _configuration = configuration;
        }

        public async Task<int> AddWorkingPattern(WorkingPatternRequest workingPatternRequest, int myId, int companyId)
        {
            var pattern = workingPatternRequest.Adapt<WorkingPattern>();
            pattern.CompanyId = companyId;
            pattern.AddedBy = myId;

            await _mainUOW.WorkingPatternRepo.AddAsync(pattern);

            await _mainUOW.SaveChangesAsync();

            return pattern.Id;
        }


        public async Task<List<WorkingPatternDto>> GetAllWorkingPatterns(string? searchTerm, int? page, int? skip, int companyId)
        {
            List<WorkingPatternDto> foundWorkingPatterns = await _mainUOW.WorkingPatternRepo.GetAllWorkingPatternsByName(searchTerm, page, skip, companyId);

            return foundWorkingPatterns;
        }


        public async Task<List<EmployeeWithContractDto>> GetEmployeesByWorkingPattern(int workingPatternId, string? searchTerm, int? page, int? skip, int myId, int companyId, int myRole, bool enableShowEmployees)
        {
            int? myIdWhenSearching = null;

            if (myRole <= StaticRoles.StaffMember && enableShowEmployees == false) return new List<EmployeeWithContractDto>();

            if (myRole == StaticRoles.Manager && enableShowEmployees == false) myIdWhenSearching = myId;

            List<EmployeeWithContractDto> employeesWithContracts = await _mainUOW.EmployeeRepo.GetEmployeesByWorkingPatternId(workingPatternId, searchTerm, page, skip, companyId, myIdWhenSearching);

            return employeesWithContracts;
        }

    }
}
