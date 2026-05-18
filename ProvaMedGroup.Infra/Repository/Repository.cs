using Microsoft.EntityFrameworkCore;
using ProvaMedGroup.DomainModel;
using ProvaMedGroup.DomainModel.Interfaces.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : EntityBase
{
    protected readonly ProvaMedGroupDbContext Db;
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(ProvaMedGroupDbContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public virtual async Task<TEntity> Read(Guid id)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public virtual async Task<IEnumerable<TEntity>> ReadAll()
    {
        return await DbSet
            .AsNoTracking()
            .ToListAsync();
    }

    public virtual void Create(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public virtual void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual async Task Delete(Guid id)
    {
        var entity = await DbSet.FirstOrDefaultAsync(x => x.Id == id);

        if (entity != null)
        {
            DbSet.Remove(entity);
        }
    }

    public void Dispose()
    {
        Db?.Dispose();
    }
}