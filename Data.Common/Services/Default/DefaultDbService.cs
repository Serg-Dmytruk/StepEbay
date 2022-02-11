using StepEbay.Data.Models.Default;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace StepEbay.Data.Common.Services.Default
{
    public class DefaultDbService<TKey, TEntity> : IDefaultDbService<TKey, TEntity>
         where TKey : IEquatable<TKey>
         where TEntity : class, IDbServiceEntity<TKey>
    {
        private readonly DbContext _db;

        public DefaultDbService(DbContext db)
        {
            _db = db;
        }

        public virtual async Task<bool> Any(TKey id)
        {
            return await _db.Set<TEntity>().AnyAsync(e => e.Id.Equals(id));
        }

        public virtual async Task<int> Count()
        {
            return await _db.Set<TEntity>().CountAsync();
        }

        public virtual async Task<List<TEntity>> List()
        {
            return await _db.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<TEntity> Get(TKey id)
        {
            return await _db.Set<TEntity>().SingleOrDefaultAsync(e => e.Id.Equals(id));
        }

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            EntityEntry entry = await _db.Set<TEntity>().AddAsync(entity);
            await _db.SaveChangesAsync();

            return (TEntity)entry.Entity;
        }

        public virtual async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _db.Set<TEntity>().AddRangeAsync(entities);
            await _db.SaveChangesAsync();
        }

        public virtual async Task Update(TEntity entity)
        {
            _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public virtual async Task UpdateRange(IEnumerable<TEntity> entities)
        {
            _db.Set<TEntity>().UpdateRange(entities);
            await _db.SaveChangesAsync();
        }

        public virtual async Task Remove(TEntity entity)
        {
            _db.Set<TEntity>().Remove(entity);
            await _db.SaveChangesAsync();
        }

        public virtual async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            _db.Set<TEntity>().RemoveRange(entities);
            await _db.SaveChangesAsync();
        }
    }
}
