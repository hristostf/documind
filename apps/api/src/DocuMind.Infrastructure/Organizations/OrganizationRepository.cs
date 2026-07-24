using DocuMind.Application.Organizations;
using DocuMind.Domain.Organizations;
using DocuMind.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DocuMind.Infrastructure.Organizations;

internal sealed class OrganizationRepository(
    DocuMindDbContext dbContext)
    : IOrganizationRepository
{
    public async Task AddAsync(
        Organization organization,
        CancellationToken cancellationToken = default)
    {
        dbContext.Organizations.Add(organization);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Organization?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Organizations
            .SingleOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }
}