using Freelando.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelando.Dados.Mapeamentos;

public class ContratoTypeConfiguration : IEntityTypeConfiguration<Contrato>
{
    public void Configure(EntityTypeBuilder<Contrato> entity)
    {
        entity.ToTable("TB_Contratos");

        entity.Property(e => e.Id)
            .HasColumnName("Id_Contrato");

        entity.Property(e => e.ServicoId)
            .HasColumnName("ID_Servico");

        entity.Property(e => e.ProfissionalId)
            .HasColumnName("ID_Profissional");

        entity.OwnsOne(e => e.Vigencia, vigencia =>
        {
            vigencia.Property(v => v.DataInicio).HasColumnName("Data_Inicio");
            vigencia.Property(v => v.DataEncerramento).HasColumnName("Data_Encerramento");
        });

        // One to One (1 contrato tem 1 serviço)
        entity
            .HasOne(c => c.Servico)
            .WithOne(s => s.Contrato)
            .HasForeignKey<Contrato>(c => c.Id);

        // One to Many (1 profissional pode ter N contratos)
        entity
           .HasOne(e => e.Profissional)
           .WithMany(e => e.Contratos)
           .HasForeignKey(e => e.ProfissionalId);
    }
}