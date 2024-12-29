using HR.Repository.Interfaces;
using HR.Subroutines;
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

        public async Task AddAsync(WorkingPattern pattern)
        {
            await _context.WorkingPatterns.AddAsync(pattern);
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                SqlExceptionHandler.ExceptionHandler(ex);
            }
        }


    }
}
