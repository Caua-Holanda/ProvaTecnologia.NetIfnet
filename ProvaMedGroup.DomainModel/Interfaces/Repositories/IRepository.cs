using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaMedGroup.DomainModel.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        void Create(TEntity entity);

        Task<TEntity> Read(Guid id);

        Task<IEnumerable<TEntity>> ReadAll();

        void Update(TEntity entity);

        Task Delete(Guid id);
    }
}