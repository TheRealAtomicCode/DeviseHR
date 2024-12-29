using HR.DTO;
using HR.Repository.Interfaces;
using HR.Services.Interfaces;
using HR.UOW.Interfaces;
using Mapster;
using Models;

namespace HR.Services
{
    public class WorkingPatternService : IWorkingPatternService
    {

        private readonly IWorkingPatternRepo _workingPatternRepo;
        private readonly IConfiguration _configuration;

        public WorkingPatternService(IWorkingPatternRepo workingPatternRepo, IConfiguration configuration)
        {
            _workingPatternRepo = workingPatternRepo;
            _configuration = configuration;
        }

        public async Task<int> AddWorkingPattern(WorkingPatternRequest workingPatternRequest, int myId, int companyId)
        {
            var pattern = workingPatternRequest.Adapt<WorkingPattern>();
            pattern.CompanyId = companyId;
            pattern.AddedBy = myId;

            await _workingPatternRepo.AddAsync(pattern);

            await _workingPatternRepo.SaveChangesAsync();

            return pattern.Id;
        }

    }
}
