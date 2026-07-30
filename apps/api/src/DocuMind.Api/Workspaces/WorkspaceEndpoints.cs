using DocuMind.Application.Workspaces.CreateWorkspace;
using DocuMind.Application.Workspaces.GetWorkspace;
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

        var result = await handler.HandleAsync(
            command,
            cancellationToken);

        if (result is null)
        {
            return Results.NotFound();
        }

        return Results.Created(
            $"/api/workspaces/{result.Id}",
            result);
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
}