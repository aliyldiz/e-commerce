using System.Linq.Expressions;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities.Common;
using ECommerceApi.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly ECommerceApiDbContext _context;
    public DbSet<T> dbSet => _context.Set<T>();
    
    public GenericRepository(ECommerceApiDbContext context)
    {
        _context = context;
    }

    public virtual async Task<int> AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
        return await _context.SaveChangesAsync();
    }

    public virtual int Add(T entity)
    {
        dbSet.Add(entity);
        return _context.SaveChanges();
    }

    public virtual int Add(IEnumerable<T> entities)
    {
        if (entities != null && !entities.Any())
            return 0;
        
        dbSet.AddRange(entities);
        return _context.SaveChanges();
    }

    public virtual async Task<int> AddAsync(IEnumerable<T> entities)
    {
        if (entities != null && !entities.Any())
            return 0;
        
        await dbSet.AddRangeAsync(entities);
        return await _context.SaveChangesAsync();
    }

    public virtual async Task<int> UpdateAsync(T entity)
    {
        dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }

    public virtual int Update(T entity)
    {
        dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return _context.SaveChanges();
    }

    public virtual Task<int> DeleteAsync(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            dbSet.Attach(entity);
        
        dbSet.Remove(entity);
        return _context.SaveChangesAsync();
    }

    public virtual int Delete(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            dbSet.Attach(entity);
        
        dbSet.Remove(entity);
        return _context.SaveChanges();
    }

    public virtual Task<int> DeleteAsync(Guid id)
    {
        var entity = dbSet.Find(id);
        return DeleteAsync(entity);
    }

    public virtual int Delete(Guid id)
    {
        var entity = dbSet.Find(id);
        if (entity != null) 
            return Delete(entity);
        
        return 0;
    }

    public virtual bool DeleteRange(Expression<Func<T, bool>> predicate)
    {
        _context.RemoveRange(dbSet.Where(predicate));
        return _context.SaveChanges() > 0;
    }

    public virtual async Task<bool> DeleteRangeAsync(Expression<Func<T, bool>> predicate)
    {
        _context.RemoveRange(dbSet.Where(predicate));
        return await _context.SaveChangesAsync() > 0;
    }

    public virtual Task<int> AddOrUpdateAsync(T entity)
    {
        if (!dbSet.Local.Any(e => EqualityComparer<Guid>.Default.Equals(e.Id, entity.Id)))
            _context.Update(entity);
        return _context.SaveChangesAsync();
    }

    public virtual int AddOrUpdate(T entity)
    {
        if (!dbSet.Local.Any(e => EqualityComparer<Guid>.Default.Equals(e.Id, entity.Id)))
            _context.Update(entity);
        return _context.SaveChanges();
    }

    public virtual IQueryable<T> AsQueryable() => dbSet.AsQueryable();

    public virtual async Task<List<T>> GetAllAsync(bool noTracking = true)
    {
        if (noTracking)
            return await dbSet.AsNoTracking().ToListAsync();
        
        return await dbSet.ToListAsync();
    }

    public virtual async Task<List<T>> GetList(Expression<Func<T, bool>> predicate, bool noTracking = true, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = dbSet;

        if (predicate != null)
            query = query.Where(predicate);

        foreach (Expression<Func<T, object>> include in includes)
            query = query.Include(include);
        
        if (orderBy != null)
            query = orderBy(query);
        
        if (noTracking)
            query = query.AsNoTracking();
        
        return await query.ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<T, object>>[] includes)
    {
        T found = await dbSet.FindAsync(id);
        
        if (found == null)
            return null;
        
        if (noTracking)
            _context.Entry(found).State = EntityState.Detached;
        
        foreach (Expression<Func<T, object>> include in includes)
            _context.Entry(found).Reference(include).Load();
        
        return found;
    }

    public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = dbSet;
        
        if (predicate != null)
            query = query.Where(predicate);
        
        query = ApplyIncludes(query, includes);
        
        if (noTracking)
            query = query.AsNoTracking();
        
        return await query.SingleOrDefaultAsync();
    }

    public virtual Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
    {
        return Get(predicate, noTracking, includes).FirstOrDefaultAsync();
    }

    public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
    {
        var query = dbSet.AsQueryable();

        if (predicate != null)
            query = query.Where(predicate);
        
        query = ApplyIncludes(query, includes);
        
        if (noTracking)
            query = query.AsNoTracking();
        
        return query;
    }

    public virtual Task BulkDeleteById(IEnumerable<Guid> ids)
    {
        if (ids != null && !ids.Any())
            return Task.CompletedTask;
        
        _context.RemoveRange(dbSet.Where(e => ids.Contains(e.Id)));
        return _context.SaveChangesAsync();
    }

    public virtual Task BulkDelete(Expression<Func<T, bool>> predicate)
    {
        _context.RemoveRange(dbSet.Where(predicate));
        return _context.SaveChangesAsync();
    }

    public virtual Task BulkDelete(IEnumerable<T> entities)
    {
        if (entities != null && !entities.Any())
            return Task.CompletedTask;
        
        dbSet.RemoveRange(entities);
        return _context.SaveChangesAsync();
    }

    public virtual Task BulkUpdate(IEnumerable<T> entities)
    {
        if (entities != null && !entities.Any())
            return Task.CompletedTask;
        
        foreach (var entityItem in entities)
        {
            dbSet.Update(entityItem);
        }
        
        return _context.SaveChangesAsync();
    }

    public virtual async Task BulkAdd(IEnumerable<T> entities)
    {
        if (entities != null && !entities.Any())
            await Task.CompletedTask;
        
        await dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }
    
    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
    
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }
    
    private static IQueryable<T> ApplyIncludes(IQueryable<T> query, params Expression<Func<T, object>>[] includes)
    {
        if (includes != null)
        {
            foreach (var includeItem in includes)
                query = query.Include(includeItem);
        }
        
        return query;
    }
}
