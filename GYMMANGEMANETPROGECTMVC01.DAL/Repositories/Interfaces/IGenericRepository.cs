using GYMMANGEMANETPROGECTMVC01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GYMMANGEMANETPROGECTMVC01.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity :BaseEntity, new()
    { 

        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        Task<TEntity> GetByIdAsync(int id, CancellationToken ct = default);
        Task<int> AddAsync(TEntity TEntity, CancellationToken ct = default);
        Task<int> UpdateAsync(TEntity TEntity, CancellationToken ct = default);
        Task<int> DeleteAsync(TEntity TEntity, CancellationToken ct = default);

        Task<bool> AnyAsync(Expression<Func<TEntity,bool>> predicate, CancellationToken ct = default);
    }
}
