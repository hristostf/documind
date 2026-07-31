using DocuMind.Application.Organizations;
using DocuMind.Domain.Workspaces;

namespace DocuMind.Application.Workspaces.CreateWorkspace;

public sealed class CreateWorkspaceHandler(
    IOrganizationRepository organizationRepository,
    IWorkspaceRepository workspaceRepository)
{
    private const int MaximumNameLength = 100;

    public async Task<CreateWorkspaceResponse> HandleAsync(
        CreateWorkspaceCommand command,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = command.Name?.Trim();

        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            return CreateWorkspaceResponse.Failure(
                CreateWorkspaceError.NameRequired);
        }

        if (normalizedName.Length > MaximumNameLength)
        {
            return CreateWorkspaceResponse.Failure(
                CreateWorkspaceError.NameTooLong);
        }

        var organization = await organizationRepository.GetByIdAsync(
            command.OrganizationId,
            cancellationToken);

        if (organization is null)
        {
            return CreateWorkspaceResponse.Failure(
                CreateWorkspaceError.OrganizationNotFound);
        }

        var workspace = new Workspace(
            Guid.NewGuid(),
            command.OrganizationId,
            normalizedName);

        await workspaceRepository.AddAsync(
            workspace,
            cancellationToken);

        var result = new CreateWorkspaceResult(
            workspace.Id,
            workspace.OrganizationId,
            workspace.Name,
            workspace.CreatedAtUtc);

        return CreateWorkspaceResponse.Success(result);
    }
}