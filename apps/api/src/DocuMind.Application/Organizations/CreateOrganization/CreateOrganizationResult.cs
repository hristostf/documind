namespace DocuMind.Application.Organizations.CreateOrganization;

public sealed record CreateOrganizationResult(
    Guid Id,
    string Name,
    DateTime CreatedAtUtc);