using Freelando.Modelos;

namespace Freelando.Modelo;

public class Contrato
{
    public Contrato() { }

    public Contrato(Guid id, double valor, Vigencia? vigencia, Servico servico, Profissional profissional)
    {
        Id = id;
        Valor = valor;
        Vigencia = vigencia;
        Servico = servico;
        Profissional = profissional;
    }

    public Guid Id { get; set; }
    public double Valor { get; set; }
    public Vigencia? Vigencia { get; set; }
    public Guid ServicoId { get; set; }

    // One to One (1 contrato tem 1 serviço)
    public Servico Servico { get; set; }

    // One to Many (1 profissional pode ter N contratos)
    public Guid ProfissionalId { get; set; }
    public Profissional Profissional { get; set; }
}