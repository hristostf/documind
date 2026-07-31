namespace DocuMind.Application.Organziations.CreateOrganization;

public sealed record CreateOrganizationResult(
    Guid Id,
    string Name,
    DateTime CreatedAtUtc);