using System;
using ProvaMed.DomainModel.Interfaces.UoW;
using ProvaMedGroup.DomainModel.Entities;
using ProvaMedGroup.DomainModel.Exceptions;
using ProvaMedGroup.DomainModel.Interfaces.Repositories;
using ProvaMedGroup.DomainModel.Interfaces.Services;
using ProvaMedGroup.DomainModel.Factories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaMedGroup.DomainService
{
    public class ContatoService : IContatoService
    {
        private IContatoRepository _ContatosRepository;
        private IUnitOfWork _unitOfWork;

        public ContatoService(IContatoRepository contatosRepository, IUnitOfWork unitOfWork)
        {
            _ContatosRepository = contatosRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Contato> AdicionarContato(Contato contato)
        {
            _ContatosRepository.Create(contato);
            await _unitOfWork.CommitAsync();
            return contato;
        }

        public async Task<Contato> AtualizarContato(Contato contato)
        {
            _ContatosRepository.Update(contato);
            await _unitOfWork.CommitAsync();
            return contato;
        }

        public async Task<Contato> AtualizarContatoAtivo(Contato contato)
        {
            contato.AlternarStatusAtivo();
            _ContatosRepository.Update(contato);
            await _unitOfWork.CommitAsync();
            return contato;
        }

        public async Task<Contato> ListarContatoId(Guid id)
        {
            var contato = await _ContatosRepository.Read(id);
            if (contato != null && contato.Ativo)
            {
                return contato;
            }
            return null;
        }

        public async Task<IEnumerable<Contato>> ListarContatos()
        {
            var data = await _ContatosRepository.ReadAll();
            return data.Where(f => f.Ativo).AsEnumerable();
        }

        public async Task<bool> DeletarContato(Guid id)
        {
            _ContatosRepository.Delete(id);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
