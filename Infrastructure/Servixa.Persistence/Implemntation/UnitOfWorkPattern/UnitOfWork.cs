using Servixa.Domain.Contracts;
using Servixa.Domain.Contracts.IGenericRepoPattern;
using Servixa.Domain.Contracts.UnitOfWorkPattern;
using Servixa.Persistence.DbContext;
using Servixa.Persistence.Implemntation.GenericRepoPattern;


namespace Servixa.Persistence.Implemntation.UnitOfWorkPattern
{
    public class UnitOfWork(ServixaDbContext _context) : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = [] ;
        public IGenericRepo<TEntity, Tkey> GetReposatory<TEntity, Tkey>() where TEntity : class, IEntity<Tkey>
        {
            var type = typeof(TEntity);
            if(_repositories.ContainsKey(type))
            {
                return (IGenericRepo<TEntity, Tkey>)_repositories[type];
            }
            var repository = new GenericRepo<TEntity, Tkey>(_context);
            _repositories[type] = repository;
            return repository;
        }
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        public ValueTask DisposeAsync() => _context.DisposeAsync();
    }
}
