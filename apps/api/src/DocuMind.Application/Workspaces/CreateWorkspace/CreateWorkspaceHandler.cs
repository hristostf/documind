using DocuMind.Application.Organizations;
using DocuMind.Application.Workspaces.CreateWorkspace;
using DocuMind.Domain.Workspaces;

namespace DocuMind.Application.Workspaces.CreateWorkspace;

public sealed class CreateWorkspaceHandler(
    IWorkspaceRepository workspaceRepository, IOrganizationRepository organizationRepository)
{
    public async Task<CreateWorkspaceResult?> HandleAsync(
        CreateWorkspaceCommand command,
        CancellationToken cancellationToken = default)
    {

        var organization = await organizationRepository.GetByIdAsync(
            command.OrganizationId,
            cancellationToken);

            if(organization is null)
            {
               
                return null;
            }

        var workspace = new Workspace(
            Guid.NewGuid(),
            command.OrganizationId,
            command.Name);
     

        await workspaceRepository.AddAsync(workspace, cancellationToken);

        return new CreateWorkspaceResult(
            workspace.Id,
            workspace.OrganizationId,
            workspace.Name,
            workspace.CreatedAtUtc);
    }
}   