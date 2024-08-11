using Freelando.Modelo;
using Freelando.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelando.Dados.Mapeamentos;

public class ProjetoTypeConfiguration : IEntityTypeConfiguration<Projeto>
{
    public void Configure(EntityTypeBuilder<Projeto> entity)
    {
        entity.ToTable("TB_Projetos");

        entity.HasKey(e => e.Id)
            .HasName("PK_Projeto");

        entity.Property(e => e.Id)
            .HasColumnName("ID_Projeto");

        entity.Property(e => e.Titulo)
            .HasColumnName("Titulo");

        entity.Property(e => e.Descricao)
            .HasColumnName("DS_Projeto")
            .HasColumnType("nvarchar(200)");

        entity.Property(e => e.Status)
            .HasConversion(
                fromObj => fromObj.ToString(),
                fromDb => (StatusProjeto)Enum.Parse(typeof(StatusProjeto), fromDb));

        // One to Many (1 cliente pode ter N projetos)
        entity
            .HasOne(p => p.Cliente)
            .WithMany(c => c.Projetos)
            .HasForeignKey("ID_Cliente");

        // Many to Many (N especialidades podem ter N projetos)
        entity
            .HasMany(e => e.Especialidades)
            .WithMany(e => e.Projetos)
            .UsingEntity<ProjetoEspecialidade>(
                l => l.HasOne<Especialidade>(e => e.Especialidade)
                        .WithMany(e => e.ProjetosEspecialidades)
                        .HasForeignKey(e => e.EspecialidadeId),
                r => r.HasOne<Projeto>(e => e.Projeto)
                        .WithMany(e => e.ProjetosEspecialidades)
                        .HasForeignKey(e => e.ProjetoId));
    }
}