using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ProjetoExtension
{
    public static void AddEndPointProjeto(this WebApplication app)
    {
        app.MapGet("/projetos", async (
            [FromServices] ProjetoConverter converter, 
            [FromServices] FreelandoContext contexto,
            int skip = 0, int take = 10) =>
        {
            var projetos = converter.EntityListToResponseList(
                [.. contexto.Projetos
                .Include(p => p.Cliente)
                .Include(p => p.Especialidades)
                .AsNoTracking()
                .Skip(skip)
                .Take(take)]);

            return Results.Ok(await Task.FromResult(projetos));
        })
        .WithTags("Projeto").WithOpenApi();

        app.MapGet("/projetos/{id}", async (
            [FromServices] ProjetoConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            var projeto = await contexto.Projetos.FindAsync(id);
            if (projeto is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(await Task.FromResult(converter.EntityToResponse(projeto)));
        })
        .WithTags("Projeto").WithOpenApi();

        app.MapPost("/projeto", async (
           [FromServices] ProjetoConverter converter,
           [FromServices] FreelandoContext contexto,
           ProjetoRequest projetoRequest) =>
        {
            var projeto = converter.RequestToEntity(projetoRequest);

            await contexto.Projetos.AddAsync(projeto);
            await contexto.SaveChangesAsync();

            return Results.Created($"/projeto/{projeto.Id}", projeto);
        })
       .WithTags("Projeto").WithOpenApi();

        app.MapPut("/projeto/{id}", async (
            [FromServices] ProjetoConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id,
            ProjetoRequest projetoRequest) =>
        {
            var projeto = await contexto.Projetos.FindAsync(id);
            if (projeto is null)
            {
                return Results.NotFound();
            }

            var projetoAtualizado = converter.RequestToEntity(projetoRequest);
            projeto.Titulo = projetoAtualizado.Titulo;
            projeto.Descricao = projetoAtualizado.Descricao;
            projeto.Status = projetoAtualizado.Status;

            await contexto.SaveChangesAsync();

            return Results.Ok((projeto));
        })
        .WithTags("Projeto").WithOpenApi();

        app.MapDelete("/projeto/{id}", async (
            [FromServices] ProjetoConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            using var transaction = contexto.Database.BeginTransaction();

            try
            {
                var projeto = await contexto.Projetos.FindAsync(id);
                if (projeto is null)
                {
                    return Results.NotFound();
                }

                contexto.Projetos.Remove(projeto);
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
        .WithTags("Projeto").WithOpenApi();

        /*app.MapDelete("/projeto/{id}", async (
            [FromServices] ProjetoConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            var projeto = await contexto.Projetos.FindAsync(id);
            if (projeto is null)
            {
                return Results.NotFound();
            }

            contexto.Projetos.Remove(projeto);
            await contexto.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Projeto").WithOpenApi();*/
    }
}