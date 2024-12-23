using HR.DTO.Inbound;
using HR.DTO;
using HR.Services.Interfaces;
using HR.UOW.Interfaces;

namespace HR.Services
{
    public class AbsenceService : IAbsenceService
    {
        private readonly IMainUOW _mainUOW;
        private readonly IConfiguration _configuration;

        public AbsenceService(IMainUOW mainUOW, IConfiguration configuration)
        {
            _mainUOW = mainUOW;
            _configuration = configuration;
        }

        public async Task<AbsenceDto> AddOrRequestAbsence(AddAbsenceRequest absenceRequest, int myId, int companyId, int userRole)
        {
            throw new NotImplementedException();
        }
    }
}
