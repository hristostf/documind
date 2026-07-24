
using DocuMind.Domain.Organizations;

namespace DocuMind.Application.Organizations.CreateOrganization;

public sealed class CreateOrganizationHandler(
    IOrganizationRepository repository)
{
    public async Task<CreateOrganizationResult> HandleAsync(
        CreateOrganizationCommand command,
        CancellationToken cancellationToken = default)
    {
        var organization = new Organization(
            Guid.NewGuid(),
            command.Name);

        await repository.AddAsync(
            organization,
            cancellationToken);

        return new CreateOrganizationResult(
            organization.Id,
            organization.Name,
            organization.CreatedAtUtc);
    }
}