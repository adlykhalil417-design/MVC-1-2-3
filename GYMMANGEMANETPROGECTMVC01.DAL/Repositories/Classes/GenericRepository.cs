

using GYMMANGEMANETPROGECTMVC01.DAL.DATA.DbContexts;
using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext=dbContext;
            _dbSet=_dbContext.Set<TEntity>();
        }

        public async Task<int> AddAsync(TEntity TEntity, CancellationToken ct = default)
        {
            _dbSet.Add(TEntity);
            return await _dbContext.SaveChangesAsync(ct);
        }
        public async Task<int> UpdateAsync(TEntity TEntity, CancellationToken ct = default)
        {
            _dbSet.Update(TEntity);

            return await _dbContext.SaveChangesAsync(ct);
        }
        public async Task<int> DeleteAsync(TEntity TEntity, CancellationToken ct = default)
        {
            _dbSet.Remove(TEntity);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();

            return await query.ToListAsync(ct);
        }

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(id, ct);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return _dbSet.AnyAsync(predicate, ct);  
        }
    }
}
