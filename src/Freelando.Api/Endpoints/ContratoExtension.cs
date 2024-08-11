using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ContratoExtension
{
    public static void AddEndPointContrato(this WebApplication app)
    {
        app.MapGet("/contratos", async (
            [FromServices] ContratoConverter converter, 
            [FromServices] FreelandoContext contexto,
            int skip = 0, int take = 10) =>
        {
            var contratos = converter.EntityListToResponseList(
                [.. contexto.Contratos
                .AsNoTracking()
                .Skip(skip)
                .Take(take)]);

            return Results.Ok(await Task.FromResult(contratos));
        })
        .WithTags("Contrato").WithOpenApi();

        app.MapGet("/contratos/{id}", async (
            [FromServices] ContratoConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            var contrato = await contexto.Contratos.FindAsync(id);
            if (contrato is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(await Task.FromResult(converter.EntityToResponse(contrato)));
        })
        .WithTags("Contrato").WithOpenApi();

        app.MapPost("/contrato", async (
            [FromServices] ContratoConverter converter,
            [FromServices] FreelandoContext contexto,
            ContratoRequest contratoRequest) =>
        {
            var contrato = converter.RequestToEntity(contratoRequest);

            await contexto.Contratos.AddAsync(contrato);
            await contexto.SaveChangesAsync();

            return Results.Created($"/contrato/{contrato.Id}", contrato);
        })
       .WithTags("Contrato").WithOpenApi();

        app.MapPut("/contrato/{id}", async (
            [FromServices] ContratoConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id,
            ContratoRequest contratoRequest) =>
        {
            var contrato = await contexto.Contratos.FindAsync(id);
            if (contrato is null)
            {
                return Results.NotFound();
            }

            var contratoAtualizado = converter.RequestToEntity(contratoRequest);
            contrato.Valor = contratoAtualizado.Valor;
            contrato.Vigencia = contratoAtualizado.Vigencia;

            await contexto.SaveChangesAsync();

            return Results.Ok((contrato));
        })
        .WithTags("Contrato").WithOpenApi();

        app.MapDelete("/contrato/{id}", async (
            [FromServices] ContratoConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            using var transaction = contexto.Database.BeginTransaction();

            try
            {
                var contrato = await contexto.Contratos.FindAsync(id);
                if (contrato is null)
                {
                    return Results.NotFound();
                }

                contexto.Contratos.Remove(contrato);
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
        .WithTags("Contrato").WithOpenApi();

        /*app.MapDelete("/contrato/{id}", async (
            [FromServices] ContratoConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            var contrato = await contexto.Contratos.FindAsync(id);
            if (contrato is null)
            {
                return Results.NotFound();
            }

            contexto.Contratos.Remove(contrato);
            await contexto.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Contrato").WithOpenApi();*/
    }
}