using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using POS.Data.Contexts;

namespace POS.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly PosExpressDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(PosExpressDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(params object[] keyValues) => await DbSet.FindAsync(keyValues);

    public async Task<IReadOnlyList<TEntity>> GetAllAsync() => await DbSet.ToListAsync();

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate) =>
        await DbSet.Where(predicate).ToListAsync();

    public IQueryable<TEntity> Query() => DbSet.AsQueryable();

    public async Task AddAsync(TEntity entity) => await DbSet.AddAsync(entity);

    public void Update(TEntity entity) => DbSet.Update(entity);

    public void Remove(TEntity entity) => DbSet.Remove(entity);
}
