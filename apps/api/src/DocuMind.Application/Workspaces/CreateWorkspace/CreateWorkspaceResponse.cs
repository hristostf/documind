namespace DocuMind.Application.Workspaces.CreateWorkspace;

public sealed record CreateWorkspaceResponse
{
    private CreateWorkspaceResponse(
        CreateWorkspaceResult? workspace,
        CreateWorkspaceError error)
    {
        Workspace = workspace;
        Error = error;
    }

    public CreateWorkspaceResult? Workspace { get; }

    public CreateWorkspaceError Error { get; }

    public bool IsSuccess =>
        Error == CreateWorkspaceError.None;

    public static CreateWorkspaceResponse Success(
        CreateWorkspaceResult workspace)
    {
        return new CreateWorkspaceResponse(
            workspace,
            CreateWorkspaceError.None);
    }

    public static CreateWorkspaceResponse Failure(
        CreateWorkspaceError error)
    {
        if (error == CreateWorkspaceError.None)
        {
            throw new ArgumentException(
                "A failure response must contain an error.",
                nameof(error));
        }

        return new CreateWorkspaceResponse(
            workspace: null,
            error);
    }
}