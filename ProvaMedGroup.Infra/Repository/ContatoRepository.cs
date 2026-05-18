using Microsoft.EntityFrameworkCore;
using ProvaMedGroup.DomainModel.Entities;
using ProvaMedGroup.DomainModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaMedGroup.Infra.Repository
{
    public class ContatoRepository : Repository<Contato>, IContatoRepository
    {
        public ContatoRepository(ProvaMedGroupDbContext context) : base(context) { }

        public async Task<IEnumerable<Contato>> GetContatos()
        {
            return await Db.Contatos.AsNoTracking()

                .ToListAsync();
        }

        public async Task<Contato> GetContato(Guid id)
        {
            return await Db.Contatos.AsNoTracking()

                .Where(f => f.Id == id).FirstOrDefaultAsync();
        }

    }
}
