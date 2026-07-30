
using DocuMind.Application.Workspaces;
using DocuMind.Domain.Workspaces;
using DocuMind.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DocuMind.Infrastructure.Workspaces;

internal sealed class WorkspaceRepository(
    DocuMindDbContext dbContext)
    : IWorkspaceRepository
{
    public async Task AddAsync(
        Workspace workspace,
        CancellationToken cancellationToken = default)
    {
        dbContext.Workspaces.Add(workspace);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Workspace?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Workspaces
            .SingleOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }
    public Task<Workspace?> GetByIdAndOrganizationIdAsync(
        Guid id,
        Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Workspaces
            .SingleOrDefaultAsync(
                workspace =>
                    workspace.Id == id &&
                    workspace.OrganizationId == organizationId,
                cancellationToken);
    }
}