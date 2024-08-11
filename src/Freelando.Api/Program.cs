using Freelando.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ServicesConfiguration(builder);

var app = builder.Build();
app.AppConfiguration();
await app.RunAsync();