namespace DocuMind.Application.Workspaces.CreateWorkspace;

public enum CreateWorkspaceError
{
    None = 0,
    OrganizationNotFound,
    NameRequired,
    NameTooLong
}