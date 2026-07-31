using DocuMind.Api.Organizations;
using DocuMind.Api.Workspaces;
using DocuMind.Application;

using DocuMind.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapOrganizationEndpoints();
app.MapWorkspaceEndpoints();

app.Run();

public partial class Program;