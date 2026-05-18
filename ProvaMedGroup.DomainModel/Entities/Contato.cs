using ProvaMedGroup.DomainModel.ValueObjects;
using ProvaMedGroup.DomainModel.Exceptions;
using ProvaMedGroup.DomainModel.Utils;

namespace ProvaMedGroup.DomainModel.Entities
{
    public class Contato : EntityBase
    {
        private Contato() { }

        public NomeCompleto NomeCompleto { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public bool Ativo { get; private set; }
        public char Sexo { get; private set; }

        public static Contato Criar(string primeiroNome, string sobrenome, DateTime dataNascimento, char sexo)
        {
            var contato = new Contato
            {
                NomeCompleto = new NomeCompleto(primeiroNome, sobrenome),
                DataNascimento = dataNascimento,
                Ativo = true, // Contato sempre começa ativo
                Sexo = sexo
            };
            contato.ValidarDataNascimento();
            return contato;
        }

        public void Atualizar(string primeiroNome, string sobrenome, DateTime dataNascimento, char sexo)
        {
            if (!Ativo)
            {
                throw new TratedExceptions("Não é possível atualizar um contato inativo.");
            }
            NomeCompleto = new NomeCompleto(primeiroNome, sobrenome);
            DataNascimento = dataNascimento;
            Sexo = sexo;
            ValidarDataNascimento();
        }

        public void AlternarStatusAtivo()
        {
            Ativo = !Ativo;
        }

        private void ValidarDataNascimento()
        {
            if (DataNascimento > DateTime.Now)
            {
                throw new TratedExceptions("A data de nascimento não pode ser futura.");
            }

            if (DataNascimento > DateTime.Now.AddYears(-1))
            {
                throw new TratedExceptions("O contato deve ter mais de 1 ano.");
            }

            if (UtilsHelper.CalcularIdade(DataNascimento) < 18)
            {
                throw new TratedExceptions("O contato deve ter no mínimo 18 anos.");
            }
        }
    }
}
