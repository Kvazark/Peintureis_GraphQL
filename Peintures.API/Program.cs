using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Peintures.Data;
using Peintures.GraphQL.Schemas;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddScoped<ISchema,PeinturesSchema>();
builder.Services.AddGraphQL(options => { options.EnableMetrics = true; }).AddSystemTextJson();
builder.Services.AddSingleton<IPeinturesStorage, PeinturesCsvFileStorage>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseGraphQLAltair();
}
app.UseRouting();
app.UseGraphQL<ISchema>();
app.Run();