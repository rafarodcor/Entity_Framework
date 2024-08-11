using Freelando.Api.Responses;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class RelatorioExtension
{
    public static void AddEndPointRelatorios(this WebApplication app)
    {
        app.MapGet("/relatorios/precoContrato", (
            [FromServices] FreelandoContext contexto,
            decimal valor) =>
        {
            return contexto.Contratos.FromSql($"SELECT * FROM dbo.TB_Contratos WHERE TB_Contratos.Valor >= {valor}").ToList();
        })
        .WithTags("Relatórios").WithOpenApi();

        app.MapGet("/relatorios/nomeCliente", (
            [FromServices] FreelandoContext contexto,
            string nomeCliente) =>
        {
            var consulta = contexto.Database.SqlQueryRaw<ClienteProjetoResponse>
            ($"SELECT " +
                $"dbo.TB_Clientes.ID_Cliente, " +
                $"dbo.TB_Clientes.Nome, " +
                $"dbo.TB_Clientes.Email, " +
                $"dbo.TB_Projetos.Titulo," +
                $"dbo.TB_Projetos.ID_Projeto, " +
                $"dbo.TB_Projetos.DS_Projeto, " +
                $"dbo.TB_Projetos.Status " +
                $"FROM dbo.TB_Clientes " +
                $"INNER JOIN dbo.TB_Projetos ON dbo.TB_Clientes.ID_Cliente = dbo.TB_Projetos.ID_Cliente " +
                $"WHERE dbo.TB_Clientes.Nome like '%{nomeCliente}%'")
            .ToList();

            return consulta;
        })
       .WithTags("Relatórios").WithOpenApi();
    }
}