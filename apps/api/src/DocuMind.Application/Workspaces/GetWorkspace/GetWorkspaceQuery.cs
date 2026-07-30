namespace DocuMind.Application.Workspaces.GetWorkspace;

public sealed record GetWorkspaceQuery(
    Guid OrganizationId,
    Guid WorkspaceId);