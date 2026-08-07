
using DocuMind.Domain.Organizations;

namespace DocuMind.Application.Organizations.CreateOrganization;

public sealed class CreateOrganizationHandler(
    IOrganizationRepository organizationRepository)
{
    private const int MaximumNameLength = 100;

    public async Task<CreateOrganizationResponse> HandleAsync(
        CreateOrganizationCommand command,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = command.Name?.Trim();

        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            return CreateOrganizationResponse.Failure(
                CreateOrganizationError.NameRequired);
        }

        if (normalizedName.Length > MaximumNameLength)
        {
            return CreateOrganizationResponse.Failure(
                CreateOrganizationError.NameTooLong);
        }

        var organization = new Organization(
            Guid.NewGuid(),
            normalizedName);

        await organizationRepository.AddAsync(
            organization,
            cancellationToken);

        var result = new CreateOrganizationResult(
            organization.Id,
            organization.Name,
            organization.CreatedAtUtc);

        return CreateOrganizationResponse.Success(result);
    }
}