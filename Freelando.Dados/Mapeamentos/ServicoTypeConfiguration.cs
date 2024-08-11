using Freelando.Modelo;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Dados.Mapeamentos;

public class ServicoTypeConfiguration : IEntityTypeConfiguration<Servico>
{
    public void Configure(EntityTypeBuilder<Servico> entity)
    {
        entity.ToTable("TB_Servicos");

        entity.Property(e => e.Id)
            .HasColumnName("ID_Servico");

        entity.Property(e => e.Descricao)
            .HasColumnName("DS_Projeto");

        entity.Property(e => e.ProjetoId)
            .HasColumnName("ID_Projeto");

        entity
            .Property(e => e.Status)
            .HasConversion(
                fromObj => fromObj.ToString(),
                fromDb => (StatusServico)Enum.Parse(typeof(StatusServico), fromDb));

        // One to One (1 contrato tem 1 serviço)
        entity
            .HasOne(s => s.Contrato)
            .WithOne(c => c.Servico);

        // One to Many (1 projeto pode ter N serviços)
        entity
            .HasOne(e => e.Projeto)
            .WithMany(e => e.Servicos)
            .HasForeignKey(e => e.ProjetoId);
    }
}