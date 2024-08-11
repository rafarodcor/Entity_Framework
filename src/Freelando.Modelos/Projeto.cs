using Freelando.Modelos;

namespace Freelando.Modelo;

public class Projeto
{
    public Projeto() { }

    public Projeto(Guid id, string? titulo, string descricao, StatusProjeto status, Cliente cliente, ICollection<Especialidade> especialidades, ICollection<Servico> servicos)
    {
        Id = id;
        Titulo = titulo;
        Descricao = descricao;
        Status = status;
        Cliente = cliente;
        Especialidades = especialidades;
        Servicos = servicos;
    }

    public Guid Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public StatusProjeto Status { get; set; }

    // One to Many (1 cliente pode ter N projetos)
    public Cliente Cliente { get; set; }

    // Many to Many (N especialidades podem ter N projetos)
    public ICollection<Especialidade> Especialidades { get; set; }
    public ICollection<ProjetoEspecialidade> ProjetosEspecialidades { get; } = [];

    // One to Many (1 projeto pode ter N serviços)
    public ICollection<Servico> Servicos { get; set; }
}