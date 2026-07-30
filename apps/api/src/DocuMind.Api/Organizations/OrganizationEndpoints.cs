using DocuMind.Application.Organizations.CreateOrganization;
using DocuMind.Application.Organizations.GetOrganization;

namespace DocuMind.Api.Organizations;

public static class OrganizationEndpoints
{
    public static IEndpointRouteBuilder MapOrganizationEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/organizations")
            .WithTags("Organizations");

        group.MapPost(
            "/",
            CreateOrganizationAsync);

        group.MapGet(
            "/{id:guid}",
            GetOrganizationAsync);

        return app;
    }

    private static async Task<IResult> CreateOrganizationAsync(
        CreateOrganizationRequest request,
        CreateOrganizationHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateOrganizationCommand(request.Name);

        var result = await handler.HandleAsync(
            command,
            cancellationToken);

        return Results.Created(
            $"/api/organizations/{result.Id}",
            result);
    }

    private static async Task<IResult> GetOrganizationAsync(
        Guid id,
        GetOrganizationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetOrganizationQuery(id);

        var result = await handler.HandleAsync(
            query,
            cancellationToken);

        return result is null
            ? Results.NotFound()
            : Results.Ok(result);
    }
}