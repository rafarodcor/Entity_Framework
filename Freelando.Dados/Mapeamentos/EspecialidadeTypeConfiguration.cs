using Freelando.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelando.Dados.Mapeamentos;

public class EspecialidadeTypeConfiguration : IEntityTypeConfiguration<Especialidade>
{
    public void Configure(EntityTypeBuilder<Especialidade> entity)
    {
        entity.ToTable("TB_Especialidades");

        entity.Property(e => e.Id)
            .HasColumnName("ID_Especialidade");

        entity.Property(e => e.Descricao)
            .HasColumnName("DS_Especialidade");
    }
}