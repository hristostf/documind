using DocuMind.Api.Organizations;
using DocuMind.Application.Organizations.CreateOrganization;
using DocuMind.Application.Organizations.GetOrganization;
using DocuMind.Infrastructure;
using DocuMind.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<CreateOrganizationHandler>();
builder.Services.AddScoped<GetOrganizationHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/health/database", async (
    DocuMindDbContext dbContext,
    CancellationToken cancellationToken) =>
{
    var canConnect =
        await dbContext.Database.CanConnectAsync(cancellationToken);

    return canConnect
        ? Results.Ok(new { status = "healthy" })
        : Results.Problem("Cannot connect to database.");
});


app.MapPost(
    "/api/organizations",
    async (
        CreateOrganizationRequest request,
        CreateOrganizationHandler handler,
        CancellationToken cancellationToken) =>
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Results.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    ["name"] = ["Organization name is required."]
                });
        }

        if (request.Name.Length > 200)
        {
            return Results.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    ["name"] = ["Organization name cannot exceed 200 characters."]
                });
        }

        var command = new CreateOrganizationCommand(
            request.Name.Trim());

        var result = await handler.HandleAsync(
            command,
            cancellationToken);

        return Results.Created(
            $"/api/organizations/{result.Id}",
            result);
    });

app.MapGet(
    "/api/organizations/{id:guid}",
    async (
           Guid id,
        GetOrganizationHandler handler,
        CancellationToken cancellationToken) =>
    {
        var query = new GetOrganizationQuery(id);

        var result = await handler.HandleAsync(
                query,
                cancellationToken);

        return result is null
            ? Results.NotFound()
            : Results.Ok(result);
    });

app.Run();