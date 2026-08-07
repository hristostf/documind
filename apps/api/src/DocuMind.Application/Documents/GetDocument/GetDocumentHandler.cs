using DocuMind.Application.Workspaces;

namespace DocuMind.Application.Documents.GetDocument;

public sealed class GetDocumentHandler(
    IDocumentRepository documentRepository,
    IWorkspaceRepository workspaceRepository)
{
    public async Task<GetDocumentResult?> HandleAsync(
        GetDocumentQuery query,
        CancellationToken cancellationToken = default)
    {
        var workspace = await workspaceRepository.GetByIdAsync(
            query.WorkspaceId,
            cancellationToken);

        if (workspace is null ||
            workspace.OrganizationId != query.OrganizationId)
        {
            return null;
        }

        var document = await documentRepository.GetByIdAsync(
            query.DocumentId,
            cancellationToken);

        if (document is null ||
            document.WorkspaceId != query.WorkspaceId)
        {
            return null;
        }

        return new GetDocumentResult(
            document.Id,
            document.WorkspaceId,
            document.Name,
            document.OriginalFileName,
            document.ContentType,
            document.SizeInBytes,
            document.Status.ToString(),
            document.CreatedAtUtc);
    }
}