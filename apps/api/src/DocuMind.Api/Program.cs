
using DocuMind.Api.Documents;
using DocuMind.Api.Organizations;
using DocuMind.Api.Workspaces;
using DocuMind.Application;

using DocuMind.Infrastructure;
using DocuMind.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.Configure<StorageOptions>(
    builder.Configuration.GetSection(StorageOptions.SectionName));
    
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.MapOrganizationEndpoints();
app.MapWorkspaceEndpoints();
app.MapDocumentEndpoints();
app.Run();

public partial class Program;