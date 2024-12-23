using HR.Repository.Interfaces;
using Models;

namespace HR.Repository
{
    public class AbsenceRepo : IAbsenceRepo
    {

        private readonly DeviseHrContext _context;
        private readonly IConfiguration _configuration;

        public AbsenceRepo(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
        }


    }
}
