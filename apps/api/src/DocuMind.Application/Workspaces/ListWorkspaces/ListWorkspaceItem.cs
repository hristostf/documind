
namespace DocuMind.Application.Workspaces.ListWorkspaces;

public sealed record ListWorkspaceItem(
    Guid Id,
    string Name,
    DateTime CreatedAtUtc);