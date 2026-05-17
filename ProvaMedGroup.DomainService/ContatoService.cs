using System;
using System.Data;
using ProvaMed.DomainModel.Interfaces.UoW;
using ProvaMedGroup.DomainModel.Entities;
using ProvaMedGroup.DomainModel.Exceptions;
using ProvaMedGroup.DomainModel.Interfaces.Repositories;
using ProvaMedGroup.DomainModel.Interfaces.Services;
using ProvaMedGroup.DomainService.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;


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

        public async Task<Contato> AdicionarContato(Contato contatos)
        {
            if (contatos.DataNascimento > DateTime.Now.AddYears(-1))
            {

                if (contatos.DataNascimento > DateTime.Now)
                {
                    throw new TratedExceptions("O contato não pode ter data de nascimento maior que hoje");
                }

                throw new TratedExceptions("O contato deve ter mais de 1 ano");
            }


            if (!UtilsHelper.MinimumAge(18, contatos.DataNascimento))
            {
                throw new TratedExceptions("O Contato deve ter no mínimo 18 anos");
            }



            contatos.Ativo = true;

            _ContatosRepository.Create(contatos);
            await _unitOfWork.CommitAsync();

            return contatos;

        }

        public async Task<Contato> AtualizarContato(Contato contatos)
        {

            if (!UtilsHelper.MinimumAge(18, contatos.DataNascimento))
            {
                throw new TratedExceptions("O Contato deve ter no mínimo 18 anos");
            }
            if (!contatos.Ativo)
            {
                throw new TratedExceptions("Esse usuario está inativo!");
            }

            _ContatosRepository.Update(contatos);
            await _unitOfWork.CommitAsync();


            return contatos;
        }

        public async Task<Contato> AtualizarContatoAtivo(Contato contatos)
        {
            if (!contatos.Ativo)
            {
                contatos.Ativo = true;
            }
            else
            {
                contatos.Ativo = false;
            }

            _ContatosRepository.Update(contatos);
            await _unitOfWork.CommitAsync();

            return contatos;
        }

        public async Task<Contato> ListarContatoId(Guid id)
        {
 
            var contato = await _ContatosRepository.Read(id);
            if (contato.Ativo)
            {
                return contato;
            }
            else
            {
                return null;
            }
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
