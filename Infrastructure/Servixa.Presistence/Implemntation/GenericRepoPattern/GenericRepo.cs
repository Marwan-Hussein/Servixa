using Microsoft.EntityFrameworkCore;
using Servixa.Domain.Contracts;
using Servixa.Domain.Contracts.IGenericRepoPattern;
using Servixa.Presistence.DbContext;


namespace Servixa.Presistence.Implemntation.GenericRepoPattern
{
    public class GenericRepo<TEntity, Tkey> : IGenericRepo<TEntity, Tkey> where TEntity : class , IEntity<Tkey>
    {
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepo(ServixaDbContext _context)
        {
            _dbSet = _context.Set<TEntity>();
        }
        public async Task<IReadOnlyList<TEntity>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();
        public async Task<TEntity?> GetByIdAsync(Tkey id) => await _dbSet.FindAsync(id);
        public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);
        public void UpdateAsync(TEntity entity) => _dbSet.Update(entity);
        public void DeleteAsync(TEntity entity) => _dbSet.Remove(entity);

    }
}
