using Servixa.Domain.Contracts.IGenericRepoPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servixa.Domain.Contracts.UnitOfWorkPattern
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepo<TEntity,Tkey> GetReposatory<TEntity, Tkey>() where TEntity : class , IEntity<Tkey>;
        Task<int> SaveChangesAsync();
    }
}
