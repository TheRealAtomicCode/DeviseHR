using HR.DTO;
using HR.Repository.Interfaces;
using HR.Subroutines;
using Mapster;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<WorkingPatternDto>> GetAllWorkingPatternsByName(string? searchTerm, int? page, int? skip, int companyId)
        {
            var query = _context.WorkingPatterns.AsQueryable();
            query = query.Where(p => p.CompanyId == companyId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.PatternName.Contains(searchTerm));
            }

            if (page.HasValue && skip.HasValue)
            {
                int skipCount = Math.Abs((page.Value - 1) * skip.Value);
                int takeCount = Math.Abs(skip.Value);
                query = query.Skip(skipCount).Take(takeCount);
            }

            return await query.ProjectToType<WorkingPatternDto>().ToListAsync();
        }

        public async Task<WorkingPatternDto?> GetWorkingPatternByIdOrDefault(int patternId, int companyId)
        {
            var workingPattern = await _context.WorkingPatterns.Where(p => p.Id == patternId && p.CompanyId == companyId).ProjectToType<WorkingPatternDto>().FirstOrDefaultAsync();

            return workingPattern;
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
