using DocuMind.Application.Organizations.CreateOrganization;
using DocuMind.Application.Organizations.GetOrganization;
using DocuMind.Application.Organziations.CreateOrganization;

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
        var command = new CreateOrganizationCommand(
            request.Name);

        var response = await handler.HandleAsync(
            command,
            cancellationToken);

        return response.Error switch
        {
            CreateOrganizationError.None =>
                Results.Created(
                    $"/api/organizations/{response.Organization!.Id}",
                    response.Organization),

            CreateOrganizationError.NameRequired =>
                Results.BadRequest(new
                {
                    error = "Organization name is required."
                }),

            CreateOrganizationError.NameTooLong =>
                Results.BadRequest(new
                {
                    error = "Organization name cannot exceed 100 characters."
                }),

            _ =>
                Results.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    detail: "An unexpected organization creation error occurred.")
        };
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