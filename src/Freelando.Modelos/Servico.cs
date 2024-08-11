namespace Freelando.Modelo;

public class Servico
{
    public Servico() { }

    public Servico(Guid id, string? titulo, string? descricao, StatusServico status, Contrato contrato, Projeto projeto, ICollection<Candidatura> candidaturas)
    {
        Id = id;
        Titulo = titulo;
        Descricao = descricao;
        Status = status;
        Contrato = contrato;
        Projeto = projeto;
        Candidaturas = candidaturas;
    }

    public Guid Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public StatusServico Status { get; set; }

    // One to One (1 contrato tem 1 serviço)
    public Contrato Contrato { get; set; }

    // One to Many (1 projeto pode ter N serviços)
    public Guid ProjetoId { get; set; }
    public Projeto Projeto { get; set; }

    // One to Many (1 serviço pode ter N candidaturas)
    public ICollection<Candidatura> Candidaturas { get; set; }
}