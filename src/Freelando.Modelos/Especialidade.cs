using Freelando.Modelos;

namespace Freelando.Modelo;

public class Especialidade
{
    public Especialidade() { }

    public Especialidade(Guid id, string? descricao, ICollection<Projeto> projetos, ICollection<Profissional> profissionais)
    {
        Id = id;
        Descricao = descricao;
        Projetos = projetos;
        Profissionais = profissionais;
    }

    public Guid Id { get; set; }
    public string? Descricao { get; set; }

    // Many to Many (N especialidades podem ter N projetos)
    public ICollection<Projeto> Projetos { get; set; }
    public ICollection<ProjetoEspecialidade> ProjetosEspecialidades { get; } = [];

    // Many to Many (N especialidades podem ter N profissionais)
    public ICollection<Profissional> Profissionais { get; set; }
    public ICollection<ProfissionalEspecialidade> ProfissionaisEspecialidades { get; } = [];
}