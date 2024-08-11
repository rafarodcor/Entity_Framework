using Freelando.Modelo;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Freelando.Modelos;

namespace Freelando.Dados.Mapeamentos;

public class ProfissionalTypeConfiguration : IEntityTypeConfiguration<Profissional>
{
    public void Configure(EntityTypeBuilder<Profissional> entity)
    {
        entity.ToTable("TB_Profissionais");

        entity.Property(e => e.Id)
            .HasColumnName("Id_Profissional");

        // Many to Many (N especialidades podem ter N profissionais)
        entity
           .HasMany(e => e.Especialidades)
           .WithMany(e => e.Profissionais)
           .UsingEntity<ProfissionalEspecialidade>(
               l => l.HasOne<Especialidade>(e => e.Especialidade)
                        .WithMany(e => e.ProfissionaisEspecialidades)
                        .HasForeignKey(e => e.EspecialidadeId),
               r => r.HasOne<Profissional>(e => e.Profissional)
                        .WithMany(e => e.ProfissionaisEspecialidades)
                        .HasForeignKey(e => e.ProfissionalId));
    }
}