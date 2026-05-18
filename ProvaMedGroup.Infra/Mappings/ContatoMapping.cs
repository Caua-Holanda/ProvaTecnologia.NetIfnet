using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProvaMedGroup.DomainModel.Entities;

namespace ProvaMedGroup.Infra.Mappings
{
    public class ContatoMapping : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.HasKey(f => f.Id);

            builder.OwnsOne(c => c.NomeCompleto, nome =>
            {
                nome.Property(n => n.PrimeiroNome)
                     .HasColumnName("PrimeiroNome")
                     .IsRequired()
                     .HasColumnType("varchar(100)");

                nome.Property(n => n.Sobrenome)
                     .HasColumnName("Sobrenome")
                     .IsRequired()
                     .HasColumnType("varchar(50)");
            });

            builder.Property(f => f.DataNascimento)
                   .IsRequired()
                   .HasColumnType("Datetime");

            builder.Property(f => f.Sexo)
                   .IsRequired()
                   .HasColumnType("char(1)");

            builder.Property(p => p.Ativo)
                   .IsRequired()
                   .HasColumnType("bit");

            builder.ToTable("Contato");
        }

    }
}
