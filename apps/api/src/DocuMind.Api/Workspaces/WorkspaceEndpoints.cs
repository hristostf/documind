using DocuMind.Application.Workspaces.CreateWorkspace;
using DocuMind.Application.Workspaces.GetWorkspace;
using DocuMind.Application.Workspaces.ListWorkspaces;
namespace DocuMind.Api.Workspaces;

public static class WorkspaceEndpoints
{
    public static IEndpointRouteBuilder MapWorkspaceEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/organizations/{organizationId:guid}/workspaces")
            .WithTags("Workspaces");

        group.MapPost(
            "/",
            CreateWorkspaceAsync);

        group.MapGet(
            "/",
            ListWorkspacesAsync);

       group.MapGet(
            "/{workspaceId:guid}",
            GetWorkspaceAsync);


        return app;
    }

private static async Task<IResult> CreateWorkspaceAsync(
    Guid organizationId,
    CreateWorkspaceRequest request,
    CreateWorkspaceHandler handler,
    CancellationToken cancellationToken)
{
    var command = new CreateWorkspaceCommand(
        organizationId,
        request.Name);

    var response = await handler.HandleAsync(
        command,
        cancellationToken);

    return response.Error switch
    {
        CreateWorkspaceError.None =>
            Results.Created(
                $"/api/organizations/{organizationId}/workspaces/{response.Workspace!.Id}",
                response.Workspace),

        CreateWorkspaceError.OrganizationNotFound =>
            Results.NotFound(),

        CreateWorkspaceError.NameRequired =>
            Results.BadRequest(new
            {
                error = "Workspace name is required."
            }),

        CreateWorkspaceError.NameTooLong =>
            Results.BadRequest(new
            {
                error = "Workspace name cannot exceed 100 characters."
            }),

        _ =>
            Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: "An unexpected workspace creation error occurred.")
    };
}

        private static async Task<IResult> GetWorkspaceAsync(
        Guid organizationId,
        Guid workspaceId,
        GetWorkspaceHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetWorkspaceQuery(
            organizationId,
            workspaceId);

        var result = await handler.HandleAsync(
            query,
            cancellationToken);

        if (result is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(result);
    }

    public static async Task<IResult> ListWorkspacesAsync(
        Guid organizationId,
        ListWorkspacesHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new ListWorkspacesQuery(organizationId);

        var result = await handler.HandleAsync(
            query,
            cancellationToken);

        if (result is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(result);
    }
}