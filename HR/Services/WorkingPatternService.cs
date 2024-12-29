using HR.Services.Interfaces;
using HR.UOW.Interfaces;
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



    }
}
