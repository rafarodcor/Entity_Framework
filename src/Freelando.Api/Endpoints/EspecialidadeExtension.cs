using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Api.Responses;
using Freelando.Dados;
using Freelando.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Freelando.Api.Endpoints;

public static class EspecialidadeExtension
{
    public static void AddEndPointEspecialidades(this WebApplication app)
    {
        app.MapGet("/especialidades", async (
            [FromServices] EspecialidadeConverter converter,
            [FromServices] FreelandoContext contexto,
            int skip = 0, int take = 10) =>
        {
            var especialidades = converter.EntityListToResponseList(
                [.. contexto.Especialidades
                .AsNoTracking()
                .Skip(skip)
                .Take(take)]);

            return Results.Ok(await Task.FromResult(especialidades));
        })
        .WithTags("Especialidade").WithOpenApi();

        app.MapGet("/especialidades/{id}", async (
            [FromServices] EspecialidadeConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            var especialidade = await contexto.Especialidades.FindAsync(id);
            if (especialidade is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(await Task.FromResult(converter.EntityToResponse(especialidade)));
        })
        .WithTags("Especialidade").WithOpenApi();

        app.MapGet("/especialidades/letra/{letraInicial}", async (
            [FromServices] EspecialidadeConverter converter,
            [FromServices] FreelandoContext contexto,
            string letraInicial) =>
        {
            Expression<Func<Especialidade, bool>> filtroExpression = null;

            if (letraInicial.Length == 1 && char.IsUpper(letraInicial[0]))
            {
                filtroExpression = especialidade => especialidade.Descricao!.StartsWith(letraInicial);
            }

            IQueryable<Especialidade> especialidades = contexto.Especialidades;

            if (filtroExpression != null)
            {
                especialidades = especialidades.Where(filtroExpression);
            }
            
            return Results.Ok(await Task.FromResult(converter.EntityListToResponseList(especialidades)));
        })
        .WithTags("Especialidade").WithOpenApi();

        app.MapPost("/especialidade", async (
            [FromServices] EspecialidadeConverter converter,
            [FromServices] FreelandoContext contexto,
            EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = converter.RequestToEntity(especialidadeRequest);

            Func<Especialidade, bool> validarDescricao = especialidade =>
                !string.IsNullOrEmpty(especialidade.Descricao) && char.IsUpper(especialidade.Descricao[0]);

            if (!validarDescricao(especialidade))
            {
                return Results.BadRequest("A descrição não pode estar em branco e deve começar com letra maiúscula.");
            }

            await contexto.Especialidades.AddAsync(especialidade);
            await contexto.SaveChangesAsync();

            return Results.Created($"/especialidade/{especialidade.Id}", especialidade);
        })
        .WithTags("Especialidade").WithOpenApi();

        app.MapPut("/especialidade/{id}", async (
            [FromServices] EspecialidadeConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id,
            EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = await contexto.Especialidades.FindAsync(id);
            if (especialidade is null)
            {
                return Results.NotFound();
            }

            var especialidadeAtualizada = converter.RequestToEntity(especialidadeRequest);
            especialidade.Descricao = especialidadeAtualizada.Descricao;
            especialidade.Projetos = especialidadeAtualizada.Projetos;

            await contexto.SaveChangesAsync();

            return Results.Ok(especialidade);
        })
        .WithTags("Especialidade").WithOpenApi();

        app.MapDelete("/especialidade/{id}", async (
           [FromServices] EspecialidadeConverter converter,
           [FromServices] FreelandoContext contexto,
           Guid id) =>
        {
            using var transaction = contexto.Database.BeginTransaction();

            try
            {
                var especialidade = await contexto.Especialidades.FindAsync(id);
                if (especialidade is null)
                {
                    return Results.NotFound();
                }

                contexto.Especialidades.Remove(especialidade);
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
       .WithTags("Especialidade").WithOpenApi();

        /*app.MapDelete("/especialidade/{id}", async (
            [FromServices] EspecialidadeConverter converter,
            [FromServices] FreelandoContext contexto,
            Guid id) =>
        {
            var especialidade = await contexto.Especialidades.FindAsync(id);
            if (especialidade is null)
            {
                return Results.NotFound();
            }

            contexto.Especialidades.Remove(especialidade);
            await contexto.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Especialidade").WithOpenApi();*/
    }
}