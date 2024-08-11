using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ProfissionalExtension
{
    public static void AddEndPointProfissional(this WebApplication app)
    {
        app.MapGet("/profissionais", async (
            [FromServices] ProfissionalConverter converter, 
            [FromServices] FreelandoContext contexto,
            int skip = 0, int take = 10) =>
        {
            var profissionais = converter.EntityListToResponseList(
                [.. contexto.Profissionais
                .Include(p => p.Especialidades)
                .AsNoTracking()
                .Skip(skip)
                .Take(take)]);

            return Results.Ok(await Task.FromResult(profissionais));
        })
        .WithTags("Profissional").WithOpenApi();

        app.MapGet("/profissionais/{id}", async (
            [FromServices] ProfissionalConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            var profissional = await contexto.Profissionais.FindAsync(id);
            if (profissional is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(await Task.FromResult(converter.EntityToResponse(profissional)));
        })
        .WithTags("Profissional").WithOpenApi();

        app.MapPost("/profissional", async (
            [FromServices] ProfissionalConverter converter,
            [FromServices] FreelandoContext contexto,
            ProfissionalRequest profissionalRequest) =>
        {
            var profissional = converter.RequestToEntity(profissionalRequest);

            await contexto.Profissionais.AddAsync(profissional);
            await contexto.SaveChangesAsync();

            return Results.Created($"/profissional/{profissional.Id}", profissional);
        })
        .WithTags("Profissional").WithOpenApi();

        app.MapPut("/profissional/{id}", async (
            [FromServices] ProfissionalConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id,
            ProfissionalRequest profissionalRequest) =>
        {
            var profissional = await contexto.Profissionais.FindAsync(id);
            if (profissional is null)
            {
                return Results.NotFound();
            }

            var profissionalAtualizado = converter.RequestToEntity(profissionalRequest);
            profissional.Nome = profissionalAtualizado.Nome;
            profissional.Cpf = profissionalAtualizado.Cpf;
            profissional.Email = profissionalAtualizado.Email;
            profissional.Telefone = profissionalAtualizado.Telefone;

            await contexto.SaveChangesAsync();

            return Results.Ok((profissional));
        })
        .WithTags("Profissional").WithOpenApi();

        app.MapDelete("/profissional/{id}", async (
            [FromServices] ProfissionalConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            using var transaction = contexto.Database.BeginTransaction();

            try
            {
                var profissional = await contexto.Profissionais.FindAsync(id);
                if (profissional is null)
                {
                    return Results.NotFound();
                }

                contexto.Profissionais.Remove(profissional);
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
        .WithTags("Profissional").WithOpenApi();

        /*app.MapDelete("/profissional/{id}", async (
            [FromServices] ProfissionalConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            var profissional = await contexto.Profissionais.FindAsync(id);
            if (profissional is null)
            {
                return Results.NotFound();
            }

            contexto.Profissionais.Remove(profissional);
            await contexto.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Profissional").WithOpenApi();*/
    }
}