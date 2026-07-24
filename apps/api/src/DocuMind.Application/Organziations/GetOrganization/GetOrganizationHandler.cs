namespace DocuMind.Application.Organizations.GetOrganization;

public sealed class GetOrganizationHandler(
    IOrganizationRepository repository)
{
    public async Task<GetOrganizationResult?> HandleAsync(
        GetOrganizationQuery query,
        CancellationToken cancellationToken = default)
    {
        var organization = await repository.GetByIdAsync(
            query.Id,
            cancellationToken);

        return organization is null
            ? null
            : new GetOrganizationResult(
                organization.Id,
                organization.Name,
                organization.CreatedAtUtc);
    }
}