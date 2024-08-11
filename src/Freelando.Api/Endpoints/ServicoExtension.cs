using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ServicoExtensions
{
    public static void AddEndPointServico(this WebApplication app)
    {
        app.MapGet("/servicos", async (
            [FromServices] ServicoConverter converter, 
            [FromServices] FreelandoContext contexto,
            int skip = 0, int take = 10) =>
        {
            var servicos = converter.EntityListToResponseList(
                [.. contexto.Servicos
                .AsNoTracking()
                .Skip(skip)
                .Take(take)]);

            return Results.Ok(await Task.FromResult(servicos));
        })
        .WithTags("Serviço").WithOpenApi();

        app.MapGet("/servicos/{id}", async (
            [FromServices] ServicoConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            var servico = await contexto.Servicos.FindAsync(id);
            if (servico is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(await Task.FromResult(converter.EntityToResponse(servico)));
        })
        .WithTags("Serviço").WithOpenApi();

        app.MapPost("/servico", async (
          [FromServices] ServicoConverter converter,
          [FromServices] FreelandoContext contexto,
          ServicoRequest servicoRequest) =>
        {
            var servico = converter.RequestToEntity(servicoRequest);

            await contexto.Servicos.AddAsync(servico);
            await contexto.SaveChangesAsync();

            return Results.Created($"/servico/{servico.Id}", servico);
        })
      .WithTags("Serviço").WithOpenApi();

        app.MapPut("/servico/{id}", async (
            [FromServices] ServicoConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id,
            ServicoRequest servicoRequest) =>
        {
            var servico = await contexto.Servicos.FindAsync(id);
            if (servico is null)
            {
                return Results.NotFound();
            }

            var servicoAtualizado = converter.RequestToEntity(servicoRequest);
            servico.Titulo = servicoAtualizado.Titulo;
            servico.Descricao = servicoAtualizado.Descricao;
            servico.Status = servicoAtualizado.Status;

            await contexto.SaveChangesAsync();

            return Results.Ok((servico));
        })
        .WithTags("Serviço").WithOpenApi();

        app.MapDelete("/servico/{id}", async (
            [FromServices] ServicoConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            using var transaction = contexto.Database.BeginTransaction();

            try
            {
                var servico = await contexto.Servicos.FindAsync(id);
                if (servico is null)
                {
                    return Results.NotFound();
                }

                contexto.Servicos.Remove(servico);
                await contexto.SaveChangesAsync();

                transaction.Commit();

                return Results.NoContent();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        })
        .WithTags("Serviço").WithOpenApi();

        /*app.MapDelete("/servico/{id}", async (
            [FromServices] ServicoConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            var servico = await contexto.Servicos.FindAsync(id);
            if (servico is null)
            {
                return Results.NotFound();
            }

            contexto.Servicos.Remove(servico);
            await contexto.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Serviço").WithOpenApi();*/
    }
}