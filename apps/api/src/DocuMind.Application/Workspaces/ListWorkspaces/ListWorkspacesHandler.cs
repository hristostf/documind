using DocuMind.Application.Organizations;

namespace DocuMind.Application.Workspaces.ListWorkspaces;

public sealed class ListWorkspacesHandler(
    IOrganizationRepository organizationRepository,
    IWorkspaceRepository workspaceRepository)
{
    public async Task<IReadOnlyList<ListWorkspaceItem>?> HandleAsync(
        ListWorkspacesQuery query,
        CancellationToken cancellationToken = default)
    {
        var organization = await organizationRepository.GetByIdAsync(
            query.OrganizationId,
            cancellationToken);

        if (organization is null)
        {
            return null;
        }

        var workspaces = await workspaceRepository.ListByOrganizationIdAsync(
            query.OrganizationId,
            cancellationToken);

        return workspaces
            .Select(workspace => new ListWorkspaceItem(
                workspace.Id,
                workspace.Name,
                workspace.CreatedAtUtc))
            .ToList();
    }
}