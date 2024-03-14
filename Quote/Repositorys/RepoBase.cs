using Microsoft.EntityFrameworkCore;
using Quote.Interfaces.RepositoryInterface;
using Quote.Interfaces.RepositoryInterface;
using Quote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quote.Repositorys
{
    public class RepoBase<T> : IRepoBase<T> where T : class
    {
        private readonly DB_SWDContext _context;
        private readonly DbSet<T> _dbSet;

        public RepoBase()
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
                throw new Exception("Error getting entities: " + ex.Message);
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting entity by ID: {ex.Message}");
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding entity: " + ex.Message);
            }
        }

        public async Task<T> AddReturnAsync(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding entity: " + ex.Message);
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                var tracker = _dbSet.Attach(entity);
                tracker.State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating entity: " + ex.Message);
            }
        }

        public async Task<T> DeleteAsync(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting entity: " + ex.Message);
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    throw new Exception($"Entity with ID {id} not found.");
                }

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting entity: " + ex.Message);
            }
        }

        public IQueryable<T> GetInclude(params string[] navigationProperties)
        {
            IQueryable<T> query = _dbSet;
            foreach (string navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }
            return query;
        }

        public async System.Threading.Tasks.Task Save()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving changes: " + ex.Message);
            }
        }


    }
}