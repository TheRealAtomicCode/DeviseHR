using HR.Repository.Interfaces;
using Models;

namespace HR.Repository
{
    public class WorkingPatternRepo : IWorkingPatternRepo
    {

        private readonly DeviseHrContext _context;
        private readonly IConfiguration _configuration;

        public WorkingPatternRepo(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
        }


    }
}
