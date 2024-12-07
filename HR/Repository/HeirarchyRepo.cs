using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq.Expressions;
using System.Linq;
using HR.Repository.Interfaces;
using HR.Subroutines;



namespace HR.Repository
{
    public class HierarchyRepo : IHierarchyRepo
    {
        private readonly DeviseHrContext _context;
        private readonly IConfiguration _configuration;

        public HierarchyRepo(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
        }

        public async Task AddHierarchy(Hierarchy hierarchy)
        {
            await _context.AddAsync(hierarchy);
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