using Microsoft.EntityFrameworkCore;
using ProvaMedGroup.DomainModel.Entities;
using ProvaMedGroup.Infra.Mappings;

public class ProvaMedGroupDbContext : DbContext
{
    public DbSet<Contato> Contatos { get; set; }

    public ProvaMedGroupDbContext(DbContextOptions<ProvaMedGroupDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging(true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ContatoMapping());
        base.OnModelCreating(modelBuilder);
    }
}