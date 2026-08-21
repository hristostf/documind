
using DocuMind.Application.Documents.Ask;
using DocuMind.Application.Documents.CreateDocument;
using DocuMind.Application.Documents.GetDocument;
using DocuMind.Application.Documents.ListDocuments;
using DocuMind.Application.Documents.Search;
using Microsoft.AspNetCore.Mvc;

namespace DocuMind.Api.Documents;

public static class DocumentEndpoints
{
    public static IEndpointRouteBuilder MapDocumentEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/organizations/{organizationId:guid}/workspaces/{workspaceId:guid}/documents")
            .WithTags("Documents");

        group.MapPost(
            "",
            CreateDocumentAsync)
            .DisableAntiforgery();;

            group.MapGet(
            "",
            ListDocumentsAsync);
            
        group.MapGet(
            "/{documentId:guid}",
            GetDocumentAsync);

        group.MapGet(
            "/search",
            SearchDocumentsAsync);

        group.MapGet(
            "/ask",
            AskDocumentsAsync);

        return app;
    }


    private static async Task<IResult> CreateDocumentAsync(
        Guid organizationId,
        Guid workspaceId,
        [FromForm] UploadDocumentRequest request,
        CreateDocumentHandler handler,
        CancellationToken cancellationToken)
    {

    await using var content =
        request.File.OpenReadStream();

    var command = new CreateDocumentCommand(
        organizationId,
        workspaceId,
        request.Name ?? Path.GetFileNameWithoutExtension(request.File.FileName),
        request.File.FileName,
        request.File.ContentType,
        request.File.Length,
        content);

        var response = await handler.HandleAsync(
            command,
            cancellationToken);

        return response.Error switch
        {
            CreateDocumentError.None =>
                Results.Created(
                    $"/api/organizations/{organizationId}/workspaces/{workspaceId}/documents/{response.Document!.Id}",
                    response.Document),

            CreateDocumentError.NameRequired =>
                Results.BadRequest(new
                {
                    error = "Document name is required."
                }),

            CreateDocumentError.OriginalNameRequired =>
                Results.BadRequest(new
                {
                    error = "Original file name is required."
                }),

            CreateDocumentError.ContentTypeRequired =>
                Results.BadRequest(new
                {
                    error = "Content type is required."
                }),

            CreateDocumentError.InvalidSizeInBytes =>
                Results.BadRequest(new
                {
                    error = "Invalid size in bytes."
                }),

            CreateDocumentError.WorkspaceNotFound =>
                Results.NotFound(new
                {
                    error = "Workspace not found."
                }),

            _ =>
                Results.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    detail: "An unexpected document creation error occurred.")
        };
    }

    private static async Task<IResult> GetDocumentAsync(
        Guid organizationId,
        Guid workspaceId,
        Guid documentId,
        GetDocumentHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetDocumentQuery(
            organizationId,
            workspaceId,
            documentId);

        var result = await handler.HandleAsync(
            query,
            cancellationToken);

        return result is null
            ? Results.NotFound()
            : Results.Ok(result);
    }

    private static async Task<IResult> ListDocumentsAsync(
        Guid organizationId,
        Guid workspaceId,
        ListDocumentsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new ListDocumentsQuery(
            organizationId,
            workspaceId);

        var result = await handler.HandleAsync(
            query,
            cancellationToken);

        return result is null
            ? Results.NotFound()
            : Results.Ok(result);
    }

    private static async Task<IResult> SearchDocumentsAsync(
        Guid organizationId,
        Guid workspaceId,
        string query,
        int limit,
        SearchDocumentsHandler handler,
        CancellationToken cancellationToken)
    {
        var searchQuery = new SearchDocumentsQuery(
            organizationId,
            workspaceId,
            query,
            limit);

        var results = await handler.HandleAsync(
            searchQuery,
            cancellationToken);

        return Results.Ok(results);
    }

    private static async Task<IResult> AskDocumentsAsync(
    Guid organizationId,
    Guid workspaceId,
    string question,
    int limit,
    AskDocumentsHandler handler,
    CancellationToken cancellationToken)
    {
        var query = new AskDocumentsQuery(
            organizationId,
            workspaceId,
            question,
            limit);

        var result = await handler.HandleAsync(
            query,
            cancellationToken);

        return result is null
            ? Results.NotFound(new
            {
                error = "Workspace not found."
            })
            : Results.Ok(result);
    }

}