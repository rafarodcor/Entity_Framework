using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Freelando.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class CandidaturaExtension
{
    public static void AddEndPointCandidatura(this WebApplication app)
    {
        app.MapGet("/candidaturas", async (
            [FromServices] CandidaturaConverter converter,
            [FromServices] FreelandoContext contexto, 
            int skip = 0, int take = 10) =>
        {
            var candidaturas = converter.EntityListToResponseList(
                [.. contexto.Candidaturas
                .AsNoTracking()
                .Skip(skip)
                .Take(take)]);

            return Results.Ok(await Task.FromResult(candidaturas));
        })
        .WithTags("Candidatura").WithOpenApi();

        app.MapGet("/candidaturas/{id}", async (
            [FromServices] CandidaturaConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            var candidatura = await contexto.Candidaturas.FindAsync(id);
            if (candidatura is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(await Task.FromResult(converter.EntityToResponse(candidatura)));
        })
        .WithTags("Candidatura").WithOpenApi();

        app.MapPost("/candidatura", async (
            [FromServices] CandidaturaConverter converter,
            [FromServices] FreelandoContext contexto,
            CandidaturaRequest candidaturaRequest) =>
        {
            var candidatura = converter.RequestToEntity(candidaturaRequest);

            await contexto.Candidaturas.AddAsync(candidatura);
            await contexto.SaveChangesAsync();

            return Results.Created($"/candidatura/{candidatura.Id}", candidatura);
        })
       .WithTags("Candidatura").WithOpenApi();

        app.MapPut("/candidatura/{id}", async (
            [FromServices] CandidaturaConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id,
            CandidaturaRequest candidaturaRequest) =>
        {
            var candidatura = await contexto.Candidaturas.FindAsync(id);
            if (candidatura is null)
            {
                return Results.NotFound();
            }

            var candidaturaAtualizada = converter.RequestToEntity(candidaturaRequest);
            candidatura.Status = candidaturaAtualizada.Status;
            candidatura.ValorProposto = candidaturaAtualizada.ValorProposto;
            candidatura.DescricaoProposta = candidaturaAtualizada.DescricaoProposta;
            candidatura.DuracaoProposta = candidaturaAtualizada.DuracaoProposta;

            await contexto.SaveChangesAsync();

            return Results.Ok((candidatura));
        })
        .WithTags("Candidatura").WithOpenApi();

        app.MapDelete("/candidatura/{id}", async (
            [FromServices] CandidaturaConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            using var transaction = contexto.Database.BeginTransaction();

            try
            {
                var candidatura = await contexto.Candidaturas.FindAsync(id);
                if (candidatura is null)
                {
                    return Results.NotFound();
                }

                contexto.Candidaturas.Remove(candidatura);
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
        .WithTags("Candidatura").WithOpenApi();

        /*app.MapDelete("/candidatura/{id}", async (
            [FromServices] CandidaturaConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            var candidatura = await contexto.Candidaturas.FindAsync(id);
            if (candidatura is null)
            {
                return Results.NotFound();
            }

            contexto.Candidaturas.Remove(candidatura);
            await contexto.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Candidatura").WithOpenApi();*/
    }
}