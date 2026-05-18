using ProvaMed.DomainModel.Interfaces.UoW;
namespace ProvaMed.Infra.UoW
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private readonly ProvaMedGroupDbContext _context;

        public EntityFrameworkUnitOfWork(ProvaMedGroupDbContext context)
        {
            this._context = context;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

      
    }
}
