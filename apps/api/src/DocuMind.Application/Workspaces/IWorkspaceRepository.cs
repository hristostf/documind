using DocuMind.Domain.Workspaces;

namespace DocuMind.Application.Workspaces;
public interface IWorkspaceRepository
{
    Task AddAsync(
        Workspace workspace,
        CancellationToken cancellationToken = default);

    Task<Workspace?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

        Task<Workspace?> GetByIdAndOrganizationIdAsync(
        Guid id,
        Guid organizationId,
        CancellationToken cancellationToken = default);
}