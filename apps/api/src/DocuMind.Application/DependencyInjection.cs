using DocuMind.Application.Documents.CreateDocument;
using DocuMind.Application.Organizations.GetOrganization;
using DocuMind.Application.Organizations.CreateOrganization;
using DocuMind.Application.Workspaces.CreateWorkspace;
using DocuMind.Application.Workspaces.GetWorkspace;
using DocuMind.Application.Workspaces.ListWorkspaces;
using Microsoft.Extensions.DependencyInjection;
using DocuMind.Application.Documents.GetDocument;
using DocuMind.Application.Documents.ListDocuments;
using DocuMind.Application.Documents.Search;
using DocuMind.Application.Documents.Ask;

namespace DocuMind.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<CreateOrganizationHandler>();
        services.AddScoped<GetOrganizationHandler>();

        services.AddScoped<CreateWorkspaceHandler>();
        services.AddScoped<GetWorkspaceHandler>();
        services.AddScoped<ListWorkspacesHandler>();

        services.AddScoped<CreateDocumentHandler>();
        services.AddScoped<GetDocumentHandler>();
        services.AddScoped<ListDocumentsHandler>();
        services.AddScoped<SearchDocumentsHandler>();
        services.AddScoped<AskDocumentsHandler>();
        return services;
    }
}