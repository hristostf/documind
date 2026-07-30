namespace DocuMind.Application.Workspaces.GetWorkspace;

public sealed record GetWorkspaceResult(
    Guid Id,
    Guid OrganizationId,
    string Name,
    DateTime CreatedAtUtc);