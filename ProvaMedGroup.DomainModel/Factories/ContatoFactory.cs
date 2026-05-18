using ProvaMedGroup.DomainModel.Entities;
using ProvaMedGroup.DomainModel.ValueObjects;

namespace ProvaMedGroup.DomainModel.Factories
{
    public static class ContatoFactory
    {
        public static Contato CriarNovoContato(string primeiroNome, string sobrenome, DateTime dataNascimento, char sexo)
        {
            return Contato.Criar(primeiroNome, sobrenome, dataNascimento, sexo);
        }
    }
}
