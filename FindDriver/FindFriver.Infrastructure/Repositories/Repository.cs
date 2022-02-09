using FindDriver.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FindFriver.Infrastructure
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public DbSet<TEntity> Entities => DbContext.Set<TEntity>();
        public DbContext DbContext { get; }

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }
        public TEntity Find(params object[] keyValues)
        {
            return Entities.Find(keyValues);
        }
        public async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await Entities.FindAsync(keyValues);
        }
        public async Task InsertAsync(TEntity entity, bool saveChanges = true)
        {
            await Entities.AddAsync(entity);
            if (saveChanges)
            {
                await DbContext.SaveChangesAsync();
            }
        }
        public async Task InsertRangeAsync(IEnumerable<TEntity> entities, bool saveChanges = true)
        {
            await DbContext.AddRangeAsync(entities);
            if (saveChanges)
            {
                await DbContext.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(int id, bool saveChanges = true)
        {
            var entity = await Entities.FindAsync(id);
            await DeleteAsync(entity);
            if (saveChanges)
            {
                await DbContext.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(TEntity entity, bool saveChanges = true)
        {
            Entities.Remove(entity);
            if (saveChanges)
            {
                await DbContext.SaveChangesAsync();
            }
        }
        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool saveChanges = true)
        {
            var enumerable = entities as TEntity[] ?? entities.ToArray();
            if (enumerable.Any())
            {
                Entities.RemoveRange(enumerable);
            }
            if (saveChanges)
            {
                await DbContext.SaveChangesAsync();
            }
        }
    }
}
