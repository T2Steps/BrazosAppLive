using BrazosApp.DataAccess.Data;
using BrazosApp.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.DataAccess.Repository
{
      public class Repository<T> : IRepository<T> where T : class 
      {
            private readonly ApplicationDbContext _db;
            internal DbSet<T> Set;
            public Repository(ApplicationDbContext db)
            {
                  _db= db;
                  Set = _db.Set<T>();
            }
            public async Task<bool> AddAsync(T entity)
            {
                  await Set.AddAsync(entity);
                  //await _db.Set<T>().AddAsync(entity);
                  return await SaveChangesAsync();
                  
            }
            public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
            {
                  IQueryable<T> query = Set;
                  if (filter != null)
                  {
                        query = query.Where(filter);
                  }
                  if (includeProperties != null)
                  {
                        foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                              query = query.Include(item);
                        }
                  }
                  if (orderBy != null)
                  {
                        return await orderBy(query).ToListAsync();
                  }
                  return await query.ToListAsync();
            }

            public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
            {
                  IQueryable<T> query = Set;
                  if (filter != null)
                  {
                        query = query.Where(filter);
                  }
                  if (includeProperties != null)
                  {
                        foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                              query = query.Include(item);
                        }
                  }
                  if (orderBy != null)
                  {
                        return orderBy(query).ToList();
                  }
                  return query.ToList();
            }

            public async Task<T> GetById(int id)
            {
                  //return await _db.Set<T>().FindAsync(id);
                  return await Set.FindAsync(id);
            }

            public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
            {
                  IQueryable<T> query = Set;
                  if (filter != null)
                  {
                        query = query.Where(filter);
                  }
                  if (includeProperties != null)
                  {
                        foreach(var item in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                        {
                             query = query.Include(item);
                        }
                  }
                  if (!isTracking)
                  {
                        query = query.AsNoTracking();
                  }

                  return await query.FirstOrDefaultAsync();
            }
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = Set;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault();
        }

        public async Task<bool> UpdateAsync(T entity)
            {
                try
                {
                    Set.Update(entity);
                }
                catch(Exception ex)
                {
                    return false;
                } 
                  return await SaveChangesAsync();
            }

            public Task<bool> Update(T entity)
            {
                  try
                  {
                        Set.Update(entity);
                  }
                  catch (Exception ex)
                  {
                        
                  }
                  return SaveChangesAsync();
            }
            public async Task<bool> RemoveAsync(T entity)
            {
                  //_db.Set<T>().Remove(entity);
                  Set.Remove(entity);
                  return await SaveChangesAsync();
            }
            public async Task<bool> SaveChangesAsync()
            {
                return await _db.SaveChangesAsync() >=0? true: false;
            }

            
      }
}
