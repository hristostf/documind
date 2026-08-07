
using DocuMind.Application.Workspaces;

namespace DocuMind.Application.Documents.ListDocuments;

public sealed class ListDocumentsHandler(
    IWorkspaceRepository workspaceRepository,
    IDocumentRepository documentRepository)
{
    public async Task<IReadOnlyList<ListDocumentItem>?> HandleAsync(
        ListDocumentsQuery query,
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

        var documents = await documentRepository.ListByWorkspaceIdAsync(
            query.WorkspaceId,
            cancellationToken);

        return documents
            .Select(document => new ListDocumentItem(
                document.Id,
                document.Name,
                document.OriginalFileName,
                document.ContentType,
                document.SizeInBytes,
                document.Status.ToString(),
                document.CreatedAtUtc))
            .ToList();
    }
}