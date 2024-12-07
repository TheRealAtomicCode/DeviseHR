using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq.Expressions;
using System.Linq;
using HR.Repository.Interfaces;



namespace HR.Repository
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly DeviseHrContext _context;
        internal DbSet<T> dbSet;
        public GenericRepo(DeviseHrContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


    }
}