using Microsoft.EntityFrameworkCore;
using Quote.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quote.Repositorys
{
    public class RepoBase<T> where T : class
    {
        private readonly DB_SWDContext _context;
        private readonly DbSet<T> _dbSet;

        protected RepoBase()
        {
            _context = new DB_SWDContext();
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting entities: " + ex);
            }
        }

        public async System.Threading.Tasks.Task AddAsync(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding entity: " + ex);
            }
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting entity: " + ex);
            }
        }

        public async System.Threading.Tasks.Task UpdateAsync(T entity)
        {
            try
            {
                var tracker = _dbSet.Attach(entity);
                tracker.State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating entity: " + ex);
            }
        }
    }
}
