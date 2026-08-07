using DocuMind.Domain.Organizations;

namespace DocuMind.Application.Organizations;

public interface IOrganizationRepository
{
    Task AddAsync(
        Organization organization,
        CancellationToken cancellationToken = default);

    Task<Organization?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}