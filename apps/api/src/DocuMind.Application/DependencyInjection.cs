using DocuMind.Application.Organizations.CreateOrganization;
using DocuMind.Application.Organizations.GetOrganization;
using DocuMind.Application.Workspaces.CreateWorkspace;
using DocuMind.Application.Workspaces.GetWorkspace;
using Microsoft.Extensions.DependencyInjection;

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
        return services;
    }
}