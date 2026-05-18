using ProvaMedGroup.DomainModel.Exceptions;

namespace ProvaMedGroup.DomainModel.ValueObjects
{
    public record NomeCompleto
    {
        public string PrimeiroNome { get; init; }
        public string Sobrenome { get; init; }

        public NomeCompleto(string primeiroNome, string sobrenome)
        {
            if (string.IsNullOrWhiteSpace( primeiroNome)) throw new TratedExceptions("Primeiro nome não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(sobrenome)) throw new TratedExceptions("Sobrenome não pode ser vazio.");

            PrimeiroNome = primeiroNome;
            Sobrenome = sobrenome;
        }

        public override string ToString() => $"{PrimeiroNome} {Sobrenome}";
    }
}
