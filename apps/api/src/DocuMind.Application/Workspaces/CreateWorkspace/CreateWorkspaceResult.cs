namespace DocuMind.Application.Workspaces.CreateWorkspace;
public sealed record CreateWorkspaceResult(
    Guid Id,
    Guid OrganizationId,
    string Name,
    DateTime CreatedAtUtc);