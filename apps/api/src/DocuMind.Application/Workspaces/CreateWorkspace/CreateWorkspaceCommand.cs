namespace DocuMind.Application.Workspaces.CreateWorkspace;

public sealed record CreateWorkspaceCommand(
    Guid OrganizationId,
    string Name);