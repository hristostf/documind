namespace DocuMind.IntegrationTests.Organizations;

public sealed record CreateOrganizationResponse(
    Guid Id,
    string Name,
    DateTime CreatedAtUtc);