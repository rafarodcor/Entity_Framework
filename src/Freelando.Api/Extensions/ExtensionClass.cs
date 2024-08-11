using Freelando.Api.Converters;
using Freelando.Api.Endpoints;
using Freelando.Dados;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Extensions;

public static class ExtensionClass
{
    public static void AppConfiguration(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.AddEndPointCandidatura();
        app.AddEndPointClientes();
        app.AddEndPointContrato();
        app.AddEndPointProfissional();
        app.AddEndPointEspecialidades();
        app.AddEndPointProjeto();
        app.AddEndPointServico();
        app.UseHttpsRedirection();
        app.AddEndPointRelatorios();
    }

    public static void ServicesConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext<FreelandoContext>((options) =>
        {
            options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
        });

        services.AddTransient<FreelandoContext>();
        services.AddTransient(typeof(CandidaturaConverter));
        services.AddTransient(typeof(ClienteConverter));
        services.AddTransient(typeof(ContratoConverter));
        services.AddTransient(typeof(EspecialidadeConverter));
        services.AddTransient(typeof(ProfissionalConverter));
        services.AddTransient(typeof(ProjetoConverter));
        services.AddTransient(typeof(ServicoConverter));
    }
}