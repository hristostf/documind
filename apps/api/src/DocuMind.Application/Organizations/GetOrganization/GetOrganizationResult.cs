namespace DocuMind.Application.Organizations.GetOrganization;

public sealed record GetOrganizationResult(
    Guid Id,
    string Name,
    DateTime CreatedAtUtc);