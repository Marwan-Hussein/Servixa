using Servixa.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servixa.Domain.Contracts.IGenericRepoPattern
{
    public interface IGenericRepo<TEntity,Tkey> where TEntity : IEntity<Tkey>
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(Tkey id);
        Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity> spec);
        Task<TEntity?> GetEntityWithSpecAsync(ISpecification<TEntity> spec);
        Task<int> CountAsync(ISpecification<TEntity> spec);
        Task AddAsync(TEntity entity);
        void UpdateAsync(TEntity entity);
        void DeleteAsync(TEntity entity);   
    }
}
