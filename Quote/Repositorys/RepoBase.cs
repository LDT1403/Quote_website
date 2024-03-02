using Microsoft.EntityFrameworkCore;
using Quote.Models;
using System;

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

        public ICollection<T> GetAll()
        {
            try
            {
                return _dbSet.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting entities: " + ex);
            }
        }
      

        public void Add(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding entity: " + ex);
            }
        }

        public bool Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting entity: " + ex);
            }
        }

        public void Update(T entity)
        {
            try
            {
                var tracker = _dbSet.Attach(entity);
                tracker.State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating entity: " + ex);
            }
        }
    }
    }
