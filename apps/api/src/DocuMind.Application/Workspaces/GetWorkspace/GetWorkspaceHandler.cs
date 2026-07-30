namespace DocuMind.Application.Workspaces.GetWorkspace;

public sealed class GetWorkspaceHandler(
    IWorkspaceRepository workspaceRepository)
{
    public async Task<GetWorkspaceResult?> HandleAsync(
        GetWorkspaceQuery query,
        CancellationToken cancellationToken = default)
    {
     var workspace =
            await workspaceRepository.GetByIdAndOrganizationIdAsync(
                query.WorkspaceId,
                query.OrganizationId,
                cancellationToken);


        if (workspace is null)
        {
            return null;
        }

        return new GetWorkspaceResult(
            workspace.Id,
            workspace.OrganizationId,
            workspace.Name,
            workspace.CreatedAtUtc);
    }
}